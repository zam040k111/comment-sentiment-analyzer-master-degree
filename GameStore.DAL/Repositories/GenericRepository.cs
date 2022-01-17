using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Extensions;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Services.Filters;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.DAL.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly GameStoreContext _context;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(GameStoreContext context)
        {
            _context = context;
            _dbSet = context.Set<T>();
        }

        public IEnumerable<T> GetAll(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            params Expression<Func<T, bool>>[] predicates)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
                query = include(query);
            
            return query.Where(predicates.ToList().CombinePredicates()).ToList();
        }

        public IEnumerable<TResult> GetAllBy<TResult>(
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            params Expression<Func<T, bool>>[] predicates)
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
                query = include(query);

            query = query.Where(predicates.ToList().CombinePredicates());

            return orderBy != null ? orderBy(query).Select(selector).ToList() : query.Select(selector).ToList();
        }

        public IEnumerable<T> GetByFilter<TFilter>(
            Pipeline<TFilter, T> pipeline,
            TFilter filter,
            params Expression<Func<T, bool>>[] predicates) =>
            pipeline.Process(filter, _dbSet.AsNoTracking().Where(predicates.ToList().CombinePredicates())).ToList();

        public void Add(T item) => _dbSet.Add(item);

        public void Update(T item) => _context.Entry(item).State = EntityState.Modified;

        public virtual void Delete(T item) => _dbSet.Remove(item);

        public TResult GetSingle<TResult>(
            Expression<Func<T, TResult>> selector = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            params Expression<Func<T, bool>>[] predicates) where TResult : class
        {
            IQueryable<T> query = _dbSet.AsNoTracking();

            if (include != null)
                query = include(query);

            query = query.Where(predicates.ToList().CombinePredicates());
            selector ??= Expression.Lambda<Func<T, TResult>>(
                Expression.Call(new Func<T, TResult>(entity => entity as TResult).Method));

            return orderBy != null ? orderBy(query).Select(selector).FirstOrDefault() : query.Select(selector).FirstOrDefault();
        }

        public void TryUpdateManyToMany<TKey>(IEnumerable<T> currentItems, IEnumerable<T> newItems, Func<T, TKey> getKey)
        {
            var enumerable = currentItems.ToList();
            var items = newItems.ToList();
            _context.Set<T>().RemoveRange(enumerable.Except(items, getKey));
            _context.Set<T>().AddRange(items.Except(enumerable, getKey));
        }

        public int Count(params Expression<Func<T, bool>>[] predicates) => _dbSet.Count(predicates.ToList().CombinePredicates());

        public IQueryable<T> GetQueryable() => _dbSet.AsNoTracking();
    }
}
