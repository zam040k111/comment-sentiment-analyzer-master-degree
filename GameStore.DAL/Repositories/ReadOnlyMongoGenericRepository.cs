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
using GameStore.DAL.Services.Filters;
using GameStore.DAL.Services.NorthwindFilters.BsonFilters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Repositories
{
    public class ReadOnlyMongoGenericRepository<T> : IReadOnlyMongoGenericRepository<T> where T : MongoModel
    {
        protected readonly IMongoCollection<T> Collection;
        protected readonly IMapper Mapper;
        protected readonly NorthwindContext Context;

        public ReadOnlyMongoGenericRepository(NorthwindContext context, IMapper mapper)
        {
            Collection = context.Db.GetCollection<T>(NortwindConfig.CollectionName[typeof(T)]);
            Mapper = mapper;
            Context = context;
        }

        public IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null)
        {
            return predicate != null
                ? Collection.Find(predicate).ToEnumerable()
                : Collection.Find(new BsonDocument()).ToEnumerable();
        }

        public IEnumerable<T> GetByFilter<TFilter>(Pipeline<TFilter, T> pipeline, TFilter filter) =>
            pipeline.Process(filter, GetQueryable()).ToList();

        public IEnumerable<T> GetByFilter(PipelineBson<T> pipeline, GameFilterEntity filter) =>
            pipeline.Process(Context.Db.GetCollection<BsonDocument>(NortwindConfig.CollectionName[typeof(T)]), filter);

        public IEnumerable<TDestination> GetAllAndMap<TDestination>(Expression<Func<T, bool>> predicate = null)
            where TDestination : SoftDeletable
        {
            throw new NotImplementedException();
        }

        public T GetSingle(Expression<Func<T, bool>> predicate) => GetQueryable().SingleOrDefault(predicate);

        public TDestination GetSingleAndMap<TDestination>(Expression<Func<T, bool>> predicate)
            where TDestination : SoftDeletable
        {
            return new Synchronizer<TDestination, T>()
                .Include(new[] { GetQueryable().SingleOrDefault(predicate) }, Context, Mapper)
                .FirstOrDefault();
        }

        public int Count(Expression<Func<T, bool>> predicate = null) => predicate != null
                ? GetQueryable().Count(predicate)
                : GetQueryable().Count();

        public IQueryable<T> GetQueryable() => Collection.AsQueryable();

    }
}
