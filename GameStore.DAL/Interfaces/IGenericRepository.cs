using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Services.Filters;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.DAL.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        IEnumerable<T> GetAll(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            params Expression<Func<T, bool>>[] predicates);

        IEnumerable<T> GetByFilter<TFilter>(
            Pipeline<TFilter, T> pipeline,
            TFilter filter,
            params Expression<Func<T, bool>>[] predicates);

        IEnumerable<TResult> GetAllBy<TResult>(
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            params Expression<Func<T, bool>>[] predicates);

        void Add(T item);
        void Update(T item);
        void Delete(T item);

        TResult GetSingle<TResult>(
            Expression<Func<T, TResult>> selector = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            params Expression<Func<T, bool>>[] predicates) where TResult : class;

        void TryUpdateManyToMany<TKey>(
            IEnumerable<T> currentItems,
            IEnumerable<T> newItems,
            Func<T, TKey> getKey);

        int Count(params Expression<Func<T, bool>>[] predicates);
        
        IQueryable<T> GetQueryable();
    }
}
