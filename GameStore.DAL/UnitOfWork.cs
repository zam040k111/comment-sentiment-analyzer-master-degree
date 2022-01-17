using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind;
using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Northwind.EntityConfigurations;
using GameStore.DAL.Repositories;
using GameStore.DAL.Services.Filters;
using GameStore.DAL.Services.NorthwindFilters.BsonFilters;

namespace GameStore.DAL
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Lazy<SoftDeletableRepository<Game>> _games;
        private readonly Lazy<SoftDeletableRepository<Genre>> _genres;
        private readonly Lazy<SoftDeletableRepository<Comment>> _comments;
        private readonly Lazy<SoftDeletableRepository<PlatformType>> _platformTypes;
        private readonly Lazy<GenericRepository<GameGenre>> _gameGenres;
        private readonly Lazy<GenericRepository<GamePlatformType>> _gamePlatformTypes;
        private readonly Lazy<SoftDeletableRepository<Publisher>> _publishers;
        private readonly Lazy<SoftDeletableRepository<OrderDetail>> _orderDetails;
        private readonly Lazy<SoftDeletableRepository<Order>> _orders;
        private readonly Lazy<SoftDeletableRepository<VisaModel>> _visaModels;
        private readonly Lazy<ReadOnlyMongoGenericRepository<Category>> _categories;
        private readonly Lazy<ReadOnlyMongoGenericRepository<OrderDetails>> _orderDetailsMongo;
        private readonly Lazy<ReadOnlyMongoGenericRepository<Orders>> _ordersMongo;
        private readonly Lazy<ReadOnlyMongoGenericRepository<Shipper>> _shippers;
        private readonly Lazy<ReadOnlyMongoGenericRepository<Supplier>> _suppliers;
        private readonly Lazy<MongoGenericRepository<Product>> _products;
        public SoftDeletableRepository<Game> GameRepository => _games.Value;
        public SoftDeletableRepository<Genre> GenreRepository => _genres.Value;
        public SoftDeletableRepository<Comment> CommentRepository => _comments.Value;
        public SoftDeletableRepository<PlatformType> PlatformTypeRepository => _platformTypes.Value;
        public GenericRepository<GameGenre> GameGenreRepository => _gameGenres.Value;
        public GenericRepository<GamePlatformType> GamePlatformTypeRepository => _gamePlatformTypes.Value;
        public SoftDeletableRepository<Publisher> PublisherRepository => _publishers.Value;
        public SoftDeletableRepository<OrderDetail> OrderDetailRepository => _orderDetails.Value;
        public SoftDeletableRepository<Order> OrderRepository => _orders.Value;
        public SoftDeletableRepository<VisaModel> VisaRepository => _visaModels.Value;
        public ReadOnlyMongoGenericRepository<Category> CategoryRepository => _categories.Value;
        public ReadOnlyMongoGenericRepository<OrderDetails> OrderDetailsRepository => _orderDetailsMongo.Value;
        public ReadOnlyMongoGenericRepository<Orders> OrdersRepository => _ordersMongo.Value;
        public ReadOnlyMongoGenericRepository<Shipper> ShipperRepository => _shippers.Value;
        public ReadOnlyMongoGenericRepository<Supplier> SupplierRepository => _suppliers.Value;
        public MongoGenericRepository<Product> ProductRepository => _products.Value;

        private readonly GameStoreContext _dbContext;
        private readonly NorthwindContext _northwindContext;
        private readonly IMapper _mapper;

        public UnitOfWork(GameStoreContext dbContext, NorthwindContext northwindContext, IMapper mapper)
        {
            _dbContext = dbContext;
            _northwindContext = northwindContext;
            _mapper = mapper;

            _games = new Lazy<SoftDeletableRepository<Game>>(() =>
                new SoftDeletableRepository<Game>(_dbContext));

            _genres = new Lazy<SoftDeletableRepository<Genre>>(() =>
                new SoftDeletableRepository<Genre>(_dbContext));

            _comments = new Lazy<SoftDeletableRepository<Comment>>(() =>
                new SoftDeletableRepository<Comment>(_dbContext));

            _platformTypes = new Lazy<SoftDeletableRepository<PlatformType>>(() =>
                new SoftDeletableRepository<PlatformType>(_dbContext));

            _gameGenres = new Lazy<GenericRepository<GameGenre>>(() =>
                new GenericRepository<GameGenre>(_dbContext));

            _gamePlatformTypes = new Lazy<GenericRepository<GamePlatformType>>(() =>
                new GenericRepository<GamePlatformType>(_dbContext));

            _publishers = new Lazy<SoftDeletableRepository<Publisher>>(() =>
                new SoftDeletableRepository<Publisher>(_dbContext));

            _orderDetails = new Lazy<SoftDeletableRepository<OrderDetail>>(() =>
                new SoftDeletableRepository<OrderDetail>(_dbContext));

            _orders = new Lazy<SoftDeletableRepository<Order>>(() =>
                new SoftDeletableRepository<Order>(_dbContext));

            _visaModels = new Lazy<SoftDeletableRepository<VisaModel>>(() =>
                new SoftDeletableRepository<VisaModel>(_dbContext));

            _categories = new Lazy<ReadOnlyMongoGenericRepository<Category>>(() =>
                new ReadOnlyMongoGenericRepository<Category>(_northwindContext, _mapper));

            _orderDetailsMongo = new Lazy<ReadOnlyMongoGenericRepository<OrderDetails>>(() =>
                new ReadOnlyMongoGenericRepository<OrderDetails>(_northwindContext, _mapper));

            _ordersMongo = new Lazy<ReadOnlyMongoGenericRepository<Orders>>(() =>
                new ReadOnlyMongoGenericRepository<Orders>(_northwindContext, _mapper));

            _shippers = new Lazy<ReadOnlyMongoGenericRepository<Shipper>>(() =>
                new ReadOnlyMongoGenericRepository<Shipper>(_northwindContext, _mapper));

            _suppliers = new Lazy<ReadOnlyMongoGenericRepository<Supplier>>(() =>
                new ReadOnlyMongoGenericRepository<Supplier>(_northwindContext, _mapper));

            _products = new Lazy<MongoGenericRepository<Product>>(() =>
                new MongoGenericRepository<Product>(_northwindContext, _mapper));
        }
        public void Save()
        {
            _dbContext.SaveChanges();
        }

        public IEnumerable<TEntity> SynchronizeDb<TEntity, TNortwindEntity>(
            Expression<Func<TEntity, bool>> predicateDb1 = null,
            Expression<Func<TNortwindEntity, bool>> predicateDb2 = null)
            where TEntity : SoftDeletable
            where TNortwindEntity : MongoModel
        {
            var firstCollection =
                ((SoftDeletableRepository<TEntity>)GetRepositoryByGenericArgumentType(typeof(TEntity)))
                .GetAll(predicates: predicateDb1);

            var secondCollection =
                ((IReadOnlyMongoGenericRepository<TNortwindEntity>)GetRepositoryByGenericArgumentType(typeof(TNortwindEntity)))
                .GetAll(predicateDb2);

            var included = new Synchronizer<TEntity, TNortwindEntity>()
                .Include(secondCollection, _northwindContext, _mapper);

            return firstCollection.Union(included);
        }

        public IEnumerable<Game> SynchronizeDbByFilter<TFilter>(
            Pipeline<TFilter, Game> pipeline,
            Pipeline<TFilter, Product> productPipeline,
            TFilter filter)
        {
            CheckKeys(ProductRepository, GameRepository);

            var result = GameRepository.GetByFilter(pipeline, filter);
            var pageSize = (filter as GameFilterEntity).PageSize;
            (filter as GameFilterEntity).PageSize -= result.Count();

            if ((filter as GameFilterEntity).PageSize > 0)
            {
                var secondCollection = ProductRepository.GetByFilter(productPipeline, filter);
                (filter as GameFilterEntity).PageSize = pageSize;
                var mapped = new Synchronizer<Game, Product>().Include(secondCollection, _northwindContext, _mapper);
                result = result.Union(mapped);
            }

            return result;
        }

        public IEnumerable<Game> SynchronizeDbByFilter<TFilter>(
            Pipeline<TFilter, Game> pipeline,
            PipelineBson<Product> pipelineBson,
            TFilter filter)
        {
            CheckKeys(ProductRepository, GameRepository);

            var result = GameRepository.GetByFilter(pipeline, filter);
            var pageSize = (filter as GameFilterEntity).PageSize;
            (filter as GameFilterEntity).PageSize -= result.Count();

            if ((filter as GameFilterEntity).PageSize > 0)
            {
                var secondCollection = ProductRepository.GetByFilter(pipelineBson, (filter as GameFilterEntity));
                (filter as GameFilterEntity).PageSize = pageSize;
                var mapped = new Synchronizer<Game, Product>().Include(secondCollection, _northwindContext, _mapper);
                result = result.Union(mapped);
            }

            return result;
        }

        private object GetRepositoryByGenericArgumentType(Type genericArgumentType)
        {
            foreach (var info in GetType().GetProperties())
            {
                if (info.PropertyType.GetGenericArguments()[0] == genericArgumentType)
                {
                    return info.GetValue(this);
                }
            }

            return null;
        }

        private void CheckKeys(
            MongoGenericRepository<Product> productRepository,
            SoftDeletableRepository<Game> gameRepository)
        {
            var products = productRepository
                .GetAll(prod => prod.Key == null || prod.Key == "")
                .ToList();

            foreach (var game in products)
            {
                var key = game.Name.Substring(0, Math.Min(5, game.Name.Length));

                var sameKeys = products
                    .Where(g => g.Key != null && g.Key.StartsWith(key))
                    .Select(g => g.Key)
                    .ToList();

                sameKeys = sameKeys.Union(gameRepository.GetAllBy(
                    g => g.Key,
                    predicates: g => g.Key.StartsWith(key))).ToList();

                var sameKey = sameKeys.OrderByDescending(k => k).FirstOrDefault();

                if (string.IsNullOrEmpty(sameKey) || !sameKey.Any(char.IsDigit) || sameKey.Equals(key))
                {
                    game.Key = key + 1;
                }
                else
                {
                    var restKey = sameKey.ToCharArray(key.Length, sameKey.Length - key.Length).Reverse().ToList();
                    var number = int.Parse(new string(restKey.TakeWhile(char.IsDigit).Reverse().ToArray()));
                    game.Key = key + (number + 1);
                }

                productRepository.Update(game);
            }
        }
    }
}
