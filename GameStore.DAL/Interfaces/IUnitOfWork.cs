using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Repositories;
using GameStore.DAL.Services.Filters;
using GameStore.DAL.Services.NorthwindFilters.BsonFilters;

namespace GameStore.DAL.Interfaces
{
    public interface IUnitOfWork
    {
        SoftDeletableRepository<Game> GameRepository { get; }
        SoftDeletableRepository<Genre> GenreRepository { get; }
        SoftDeletableRepository<Comment> CommentRepository { get; }
        SoftDeletableRepository<PlatformType> PlatformTypeRepository { get; }
        GenericRepository<GameGenre> GameGenreRepository { get; }
        GenericRepository<GamePlatformType> GamePlatformTypeRepository { get; }
        SoftDeletableRepository<Publisher> PublisherRepository { get; }
        SoftDeletableRepository<OrderDetail> OrderDetailRepository { get; }
        SoftDeletableRepository<Order> OrderRepository { get; }
        SoftDeletableRepository<VisaModel> VisaRepository { get; }
        ReadOnlyMongoGenericRepository<Category> CategoryRepository { get; }
        ReadOnlyMongoGenericRepository<OrderDetails> OrderDetailsRepository { get; }
        ReadOnlyMongoGenericRepository<Orders> OrdersRepository { get; }
        ReadOnlyMongoGenericRepository<Shipper> ShipperRepository { get; }
        ReadOnlyMongoGenericRepository<Supplier> SupplierRepository { get; }
        MongoGenericRepository<Product> ProductRepository { get; }

        void Save();

        IEnumerable<TEntity> SynchronizeDb<TEntity, TNortwindEntity>(
            Expression<Func<TEntity, bool>> predicateDb1 = null,
            Expression<Func<TNortwindEntity, bool>> predicateDb2 = null)
            where TEntity : SoftDeletable
            where TNortwindEntity : MongoModel;

        IEnumerable<Game> SynchronizeDbByFilter<TFilter>(
            Pipeline<TFilter, Game> pipeline,
            Pipeline<TFilter, Product> productPipeline,
            TFilter filter);

        IEnumerable<Game> SynchronizeDbByFilter<TFilter>(
            Pipeline<TFilter, Game> pipeline,
            PipelineBson<Product> pipelineBson,
            TFilter filter);
    }
}

