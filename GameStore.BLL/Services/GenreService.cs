using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.BLL.DTO;
using GameStore.BLL.Exceptions.ServiceExceptions;
using GameStore.BLL.Interfaces;
using GameStore.BLL.Services.Validation;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace GameStore.BLL.Services
{
    public class GenreService : IGenreService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public GenreService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public Result<GenreDto> Add(GenreDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid || result.IsRestored)
            {
                return result;
            }

            var newGenre = _mapper.Map<Genre>(itemDto);

            _unitOfWork.GenreRepository.Add(newGenre);
            _unitOfWork.Save();
            result.Value = _mapper.Map<GenreDto>(newGenre);

            return result;
        }

        public Result<GenreDto> Update(GenreDto itemDto)
        {
            var result = CheckValidity(itemDto);

            if (!result.IsValid)
            {
                return result;
            }

            var newGenre = _mapper.Map<Genre>(itemDto);

            _unitOfWork.GameGenreRepository.TryUpdateManyToMany(_unitOfWork.GameGenreRepository
                .GetAll(predicates: gameGenre => gameGenre.GenreId == newGenre.Id),
                newGenre.GameGenres.Select(gameGenre => new GameGenre
                {
                    GameId = gameGenre.GameId,
                    GenreId = newGenre.Id
                }), gameGenre => gameGenre.GameId);
            
            _unitOfWork.GenreRepository.Update(newGenre);
            _unitOfWork.Save();
            result.Value = _mapper.Map<GenreDto>(newGenre);

            return result;
        }

        public void Delete(int id)
        {
            var genreItem = _unitOfWork.GenreRepository.GetSingle(genre => genre, include: genreInclude => genreInclude
                    .Include(genre => genre.GameGenres)
                    .Include(genre => genre.ParentGenre),
                predicates: genre => genre.Id == id);

            if (genreItem == null)
            {
                throw new NotFoundException();
            }

            _unitOfWork.GenreRepository.Delete(genreItem);
            _unitOfWork.Save();
        }

        public List<GenreDto> GetAll(bool includeIsDeleted = false)
        {
            var result = _unitOfWork.GenreRepository
                .GetAll(genreInclude => genreInclude
                    .Include(genre => genre.GameGenres)
                    .Include(genre => genre.ParentGenre),
                    includeIsDeleted).ToList();

            return _mapper.Map<List<GenreDto>>(result);
        }

        public GenreDto GetById(int id)
        {
            var result = _unitOfWork.GenreRepository
                .GetSingle(genre => genre, include: genreInclude => genreInclude
                    .Include(genre => genre.GameGenres)
                    .Include(genre => genre.ParentGenre),
                    predicates: genre => genre.Id == id);

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<GenreDto>(result);
        }

        public GenreDto GetByName(string name)
        {
            var result = _unitOfWork.GenreRepository
                .GetSingle(genre => genre, include: genreInclude => genreInclude
                    .Include(genre => genre.GameGenres)
                    .Include(genre => genre.ParentGenre),
                    predicates: genre => genre.Name.Equals(name));

            if (result == null)
            {
                throw new NotFoundException();
            }

            return _mapper.Map<GenreDto>(result);
        }

        public List<GenreDto> GetByGame(int gameId)
        {
            var result = _unitOfWork.GenreRepository
                .GetAll(genreInclude => genreInclude
                        .Include(genre => genre.GameGenres)
                        .Include(genre => genre.ParentGenre),
                    predicates: genre => genre.GameGenres
                        .Any(gameGenre => gameGenre.GameId == gameId)).ToList();

            return _mapper.Map<List<GenreDto>>(result);
        }

        private Result<GenreDto> CheckValidity(GenreDto itemDto)
        {
            var result = new Result<GenreDto> { Value = itemDto };
            var genre = _unitOfWork.GenreRepository.GetSingle(
                gnr => gnr,
                predicates: gm => gm.Name.Equals(itemDto.Name),
                includeDeleted: true);

            if (genre != null && itemDto.Id != genre.Id && itemDto.ParentGenreId == genre.ParentGenreId)
            {
                if (genre.IsDeleted)
                {
                    _unitOfWork.GenreRepository.Restore(genre);
                    result.IsRestored = true;
                    return result;
                }

                result.Errors.Add(itemDto.GetPropName(i => i.Name), itemDto.GetMessage(p => p.Name));
                return result;
            }

            return result;
        }
    }
}