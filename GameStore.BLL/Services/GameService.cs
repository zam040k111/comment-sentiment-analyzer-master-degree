using System;
using System.Linq;
using System.Collections.Generic;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Validation;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Services.Filters;
using GameStore.DAL.Services.NorthwindFilters;
using GameStore.DAL.Services.NorthwindFilters.BsonFilters;
using Microsoft.EntityFrameworkCore;
using NameFilter = GameStore.DAL.Services.Filters.NameFilter;
using PriceFilter = GameStore.DAL.Services.Filters.PriceFilter;
using SortByFilter = GameStore.DAL.Services.Filters.SortByFilter;

namespace GameStore.BLL.Services
{
    public class GameService : IGameService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GameService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }


        public Result<GameDto> Add(GameDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var game = _mapper.Map<Game>(itemDto);
            game.Published = DateTime.Now;
            _unitOfWork.GameRepository.Add(game);
            _unitOfWork.Save();
            result.Value = _mapper.Map<GameDto>(game);

            return result;
        }

        public List<GameDto> GetAll(bool includeIsDeleted = false)
        {
            var result = _unitOfWork.GameRepository
                .GetAll(gameInclude => gameInclude
                    .Include(game => game.GameGenres)
                    .Include(game => game.GamePlatformTypes),
                    includeIsDeleted);

            if (!includeIsDeleted)
            {
                result.ToList().ForEach(ExcludeIsDeletedIncludes);
            }

            return _mapper.Map<List<GameDto>>(result);
        }


        public Result<GameDto> Update(GameDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var newGame = _mapper.Map<Game>(itemDto);

            _unitOfWork.GameGenreRepository.TryUpdateManyToMany(_unitOfWork.GameGenreRepository
                    .GetAll(predicates: gameGenre => gameGenre.GameId == newGame.Id),
                newGame.GameGenres.Select(gameGenre => new GameGenre
                {
                    GenreId = gameGenre.GenreId,
                    GameId = newGame.Id
                }), gameGenre => gameGenre.GenreId);

            _unitOfWork.GamePlatformTypeRepository.TryUpdateManyToMany(_unitOfWork.GamePlatformTypeRepository
                    .GetAll(predicates: gamePlatformType => gamePlatformType.GameId == newGame.Id),
                newGame.GamePlatformTypes.Select(gamePlatformType => new GamePlatformType
                {
                    PlatformTypeId = gamePlatformType.PlatformTypeId,
                    GameId = newGame.Id
                }), gamePlatformType => gamePlatformType.PlatformTypeId);

            _unitOfWork.GameRepository.Update(newGame);
            _unitOfWork.Save();
            result.Value = _mapper.Map<GameDto>(newGame);

            return result;
        }

        public void Delete(int id)
        {
            var gameItem = _unitOfWork.GameRepository.GetSingle(game => game, predicates: game => game.Id == id);

            if (gameItem == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.GameRepository.Delete(gameItem);
            _unitOfWork.Save();
        }

        public GameDto GetById(int id)
        {
            var result = _unitOfWork.GameRepository.GetSingle(game => game, include: gameInclude => gameInclude
                    .Include(game => game.GameGenres)
                    .ThenInclude(genre => genre.Genre)
                    .Include(game => game.GamePlatformTypes)
                    .ThenInclude(type => type.PlatformType)
                    .Include(pub => pub.Publisher),
                predicates: game => game.Id == id);

            if (result == null)
            {
                throw new NotFoundException();
            }

            ExcludeIsDeletedIncludes(result);

            return _mapper.Map<GameDto>(result);
        }

        public GameFilterDto GetAll(int pageSize, int pageNumber, bool includeIsDeleted = false)
        {
            var filterModel = new GameFilterDto { PageSize = pageSize, PageNumber = pageNumber };
            var pipeLine = new GamePipeline();

            filterModel.GamesPerPage = _mapper.Map<List<GameDto>>(
                _unitOfWork.GameRepository.GetByFilter(
                    pipeLine,
                    _mapper.Map<GameFilterEntity>(filterModel)));

            filterModel.TotalItems = pipeLine.TotalItems;

            return filterModel;
        }

        public GameDto GetByKey(string key)
        {
            var result = _unitOfWork.GameRepository.GetSingle(game => game, include: gameInclude => gameInclude
                    .Include(game => game.GameGenres)
                    .ThenInclude(genre => genre.Genre)
                    .Include(game => game.GamePlatformTypes)
                    .ThenInclude(type => type.PlatformType)
                    .Include(pub => pub.Publisher),
                predicates: game => game.Key.Equals(key));

            if (result == null)
            {
                throw new NotFoundException();
            }

            ExcludeIsDeletedIncludes(result);
            result.Viewed++;
            _unitOfWork.GameRepository.Update(result);
            _unitOfWork.Save();

            return _mapper.Map<GameDto>(result);
        }

        public GameDto GetByKeyFromBothDb(string key)
        {
            var result = _unitOfWork.GameRepository.GetSingle(game => game, include: gameInclude => gameInclude
                    .Include(game => game.GameGenres)
                    .ThenInclude(genre => genre.Genre)
                    .Include(game => game.GamePlatformTypes)
                    .ThenInclude(type => type.PlatformType)
                    .Include(pub => pub.Publisher),
                predicates: game => game.Key.Equals(key));

            if (result == null)
            {
                var game = _unitOfWork.ProductRepository.GetSingleAndMap<Game>(prod => prod.Key == key);

                if (game == null)
                {
                    throw new NotFoundException();
                }

                return _mapper.Map<GameDto>(game);
            }
            else
            {
                ExcludeIsDeletedIncludes(result);
                result.Viewed++;
                _unitOfWork.GameRepository.Update(result);
                _unitOfWork.Save();

                return _mapper.Map<GameDto>(result);
            }
        }

        public List<GameDto> GetByGenre(int genreId)
        {
            var result = _unitOfWork.GameRepository
                .GetAll(gameInclude => gameInclude
                        .Include(game => game.GameGenres)
                        .ThenInclude(gameGenre => gameGenre.Genre)
                        .Include(game => game.GamePlatformTypes)
                        .ThenInclude(gamePlatformType => gamePlatformType.PlatformType),
                    predicates: game => game.GameGenres
                        .Any(gameGenre => gameGenre.GenreId == genreId)).ToList();

            return _mapper.Map<List<GameDto>>(result);
        }

        public List<GameDto> GetByPlatformType(int platformTypeId)
        {
            var result = _unitOfWork.GameRepository
                .GetAll(gameInclude => gameInclude
                        .Include(game => game.GameGenres)
                        .ThenInclude(gameGenre => gameGenre.Genre)
                        .Include(game => game.GamePlatformTypes)
                        .ThenInclude(gamePlatformType => gamePlatformType.PlatformType),
                    predicates: game => game.GamePlatformTypes
                        .Any(gamePlatformType => gamePlatformType.PlatformTypeId == platformTypeId)).ToList();

            return _mapper.Map<List<GameDto>>(result);
        }

        public List<GameDto> GetByPublisher(int publisherId)
        {
            var result = _unitOfWork.GameRepository
                .GetAll(gameInclude => gameInclude
                        .Include(game => game.GameGenres)
                        .ThenInclude(gameGenre => gameGenre.Genre)
                        .Include(game => game.GamePlatformTypes)
                        .ThenInclude(gamePlatformType => gamePlatformType.PlatformType)
                        .Include(publisher => publisher.Publisher),
                    predicates: game => game.Publisher.Id == publisherId).ToList();

            return _mapper.Map<List<GameDto>>(result);
        }

        public GameFilterDto GetAllFromBothDb(int pageSize, int pageNumber, bool includeIsDeleted = false)
        {
            var filterModel = new GameFilterDto { PageSize = pageSize, PageNumber = pageNumber };
            var pipeLine = new GamePipeline();
            var productPipeline = new ProductPipeline();

            filterModel.GamesPerPage = _mapper.Map<List<GameDto>>(
                _unitOfWork.SynchronizeDbByFilter(
                    pipeLine,
                    productPipeline,
                    _mapper.Map<GameFilterEntity>(filterModel)));

            filterModel.TotalItems = pipeLine.TotalItems + productPipeline.TotalItems;

            return filterModel;
        }

        public GameFilterDto ApplyFilter(GameFilterDto filterModel)
        {
            var pipeLine = new GamePipeline()
                .Register(new GenreFilter())
                .Register(new PlatformsFilter())
                .Register(new PublisherFilter())
                .Register(new NameFilter())
                .Register(new PriceFilter())
                .Register(new DateFilter())
                .Register(new SortByFilter());

            filterModel.GamesPerPage = _mapper.Map<List<GameDto>>(
                _unitOfWork.GameRepository.GetByFilter(
                    pipeLine,
                    _mapper.Map<GameFilterEntity>(filterModel)));

            filterModel.TotalItems = pipeLine.TotalItems;

            return filterModel;
        }

        public GameFilterDto ApplyFilterBothBd(GameFilterDto filterModel)
        {
            var filter = _mapper.Map<GameFilterEntity>(filterModel);
            var pipeline = new GamePipeline()
                .Register(new GenreFilter())
                .Register(new PlatformsFilter())
                .Register(new PublisherFilter())
                .Register(new NameFilter())
                .Register(new PriceFilter())
                .Register(new DateFilter())
                .Register(new SortByFilter());

            //var productPipeline = new ProductPipeline();

            //productPipeline
            //    .Register(new CategoryFilter())
            //    .Register(new SupllierFilter())
            //    .Register(new DAL.Services.NorthwindFilters.NameFilter())
            //    .Register(new DAL.Services.NorthwindFilters.PriceBsonFilter())
            //    .Register(new DAL.Services.NorthwindFilters.SortByFilter());

            //filterModel.GamesPerPage = _mapper.Map<List<GameDto>>(
            //    _unitOfWork.SynchronizeDbByFilter(
            //        pipeLine,
            //        productPipeline,
            //        _mapper.Map<GameFilterEntity>(filterModel)));

            //filterModel.TotalItems = pipeLine.TotalItems + productPipeline.TotalItems;

            var pipelineBson = new PipelineBson<Product>()
                .Register(new CategoryBsonFilter())
                .Register(new SupplierBsonFilter())
                .Register(new NameBsonFilter())
                .Register(new PriceBsonFilter())
                .Register(new SortBsonFilter());

            filterModel.GamesPerPage = _mapper.Map<List<GameDto>>(
                _unitOfWork.SynchronizeDbByFilter(pipeline, pipelineBson, filter));

            filterModel.TotalItems = pipeline.TotalItems + pipelineBson.TotalItems;

            return filterModel;
        }

        private Result<GameDto> CheckValidity(GameDto itemDto)
        {
            var result = new Result<GameDto> { Value = itemDto };

            if (string.IsNullOrEmpty(itemDto.Key))
            {
                result.Value.Key = KeyGenerator.GenerateKey(itemDto, _unitOfWork.GameRepository);
            }

            var game = _unitOfWork.GameRepository.GetSingle(g => g, predicates: gm => gm.Key.Equals(itemDto.Key), includeDeleted: true);

            if (game != null && itemDto.Id != game.Id)
            {
                result.Errors.Add(itemDto.GetPropName(p => p.Key), itemDto.GetMessage(m => m.Key));
                return result;
            }

            if (_unitOfWork.PublisherRepository.GetSingle(g => g, predicates: pr => pr.Id == itemDto.PublisherId) == null)
            {
                result.Errors.Add(itemDto.GetPropName(p => p.PublisherId), itemDto.GetMessage(m => m.PublisherId));
                return result;
            }

            var platformTypeCount = _unitOfWork.PlatformTypeRepository.Count(
                plt => itemDto.GamePlatformTypesId.Contains(plt.Id));

            if (platformTypeCount != itemDto.GamePlatformTypesId.Count)
            {
                result.Errors.Add(itemDto.GetPropName(p => p.GamePlatformTypesId),
                    itemDto.GetMessage(m => m.GamePlatformTypesId));
                return result;
            }

            if (itemDto.GameGenresId != null)
            {
                var genresCount = _unitOfWork.GenreRepository.Count(
                    genre => itemDto.GameGenresId.Contains(genre.Id));

                if (genresCount != itemDto.GameGenresId.Count)
                {
                    result.Errors.Add(itemDto.GetPropName(p => p.GameGenresId),
                        itemDto.GetMessage(m => m.GameGenresId));
                }
            }

            return result;
        }

        private void ExcludeIsDeletedIncludes(Game game)
        {
            if (game == null) return;

            if (game.Publisher != null && game.Publisher.IsDeleted)
            {
                game.Publisher = null;
                game.PublisherId = null;
            }

            game.GameGenres = game.GameGenres.ToList().Where(i => i.Genre != null && !i.Genre.IsDeleted).ToList();
            game.GamePlatformTypes = game.GamePlatformTypes.ToList()
                .Where(i => i.PlatformType != null && !i.PlatformType.IsDeleted).ToList();
        }

        public int Count()
        {
            return _unitOfWork.GameRepository.Count();
        }

        public List<GameDto> GetBest(int count)
        {
            var result = _unitOfWork.GameRepository.GetAllBy(
                game => game,
                game => game.OrderByDescending(key => key.Score),
                predicates: game => game.Score != 0)
                .Take(count);

            return _mapper.Map<List<GameDto>>(result);
        }
    }
}
