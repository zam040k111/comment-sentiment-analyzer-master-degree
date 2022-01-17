using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Services.Filters;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.DAL.Interfaces
{
    public interface ISoftDeletableRepository<T> : IGenericRepository<T> where T : class
    {
        void Restore(T item);

        IEnumerable<T> GetAll(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates);

        IEnumerable<T> GetByFilter<TFilter>(
            Pipeline<TFilter, T> pipeline,
            TFilter filter,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates);

        TResult GetSingle<TResult>(
            Expression<Func<T, TResult>> selector = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates) where TResult : class;

        IEnumerable<TResult> GetAllBy<TResult>(
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates);

        int Count(bool includeDeleted = false, params Expression<Func<T, bool>>[] predicates);
    }
}
