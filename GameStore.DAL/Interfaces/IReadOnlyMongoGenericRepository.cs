using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.DAL.Services.Filters;
using GameStore.DAL.Services.NorthwindFilters.BsonFilters;

namespace GameStore.DAL.Interfaces
{
    public interface IReadOnlyMongoGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(Expression<Func<T, bool>> predicate = null);
        IEnumerable<TDestination> GetAllAndMap<TDestination>(Expression<Func<T, bool>> predicate = null)
            where TDestination : SoftDeletable;
        T GetSingle(Expression<Func<T, bool>> predicate);
        TDestination GetSingleAndMap<TDestination>(Expression<Func<T, bool>> predicate)
            where TDestination : SoftDeletable;
        IEnumerable<T> GetByFilter<TFilter>(Pipeline<TFilter, T> pipeline, TFilter filter);
        IEnumerable<T> GetByFilter(PipelineBson<T> pipeline, GameFilterEntity filter);
        int Count(Expression<Func<T, bool>> predicate = null);
        public IQueryable<T> GetQueryable();
    }
}
