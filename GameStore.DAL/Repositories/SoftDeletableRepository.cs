using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Services.Filters;
using Microsoft.EntityFrameworkCore.Query;

namespace GameStore.DAL.Repositories
{
    public class SoftDeletableRepository<T> : GenericRepository<T>, ISoftDeletableRepository<T> where T : SoftDeletable
    {
        public SoftDeletableRepository(GameStoreContext context) : base(context) { }

        public override void Delete(T item)
        {
            item.IsDeleted = true;
            Update(item);
        }
        public void Restore(T item)
        {
            item.IsDeleted = false;
            Update(item);
        }

        public IEnumerable<T> GetAll(
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates)
        {
            if (!includeDeleted)
            {
                var predicatesList = predicates.ToList();
                predicatesList.Add(entity => !entity.IsDeleted);
                predicates = predicatesList.ToArray();
            }

            return base.GetAll(include, predicates);
        }

        public IEnumerable<T> GetByFilter<TFilter>(
            Pipeline<TFilter, T> pipeline,
            TFilter filter,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates)
        {
            if (!includeDeleted)
            {
                var predicatesList = predicates.ToList();
                predicatesList.Add(entity => !entity.IsDeleted);
                predicates = predicatesList.ToArray();
            }

            return base.GetByFilter(pipeline, filter, predicates);
        }

        public TResult GetSingle<TResult>(
            Expression<Func<T, TResult>> selector = null,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates) where TResult : class
        {
            if (!includeDeleted)
            {
                var predicatesList = predicates.ToList();
                predicatesList.Add(entity => !entity.IsDeleted);
                predicates = predicatesList.ToArray();
            }

            return base.GetSingle(selector, orderBy, include, predicates);
        }

        public IEnumerable<TResult> GetAllBy<TResult>(
            Expression<Func<T, TResult>> selector,
            Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null,
            Func<IQueryable<T>, IIncludableQueryable<T, object>> include = null,
            bool includeDeleted = false,
            params Expression<Func<T, bool>>[] predicates)
        {
            if (!includeDeleted)
            {
                var predicatesList = predicates.ToList();
                predicatesList.Add(entity => !entity.IsDeleted);
                predicates = predicatesList.ToArray();
            }

            return base.GetAllBy(selector, orderBy, include, predicates);
        }

        public int Count(bool includeDeleted = false, params Expression<Func<T, bool>>[] predicates)
        {
            if (!includeDeleted)
            {
                var predicatesList = predicates.ToList();
                predicatesList.Add(entity => !entity.IsDeleted);
                predicates = predicatesList.ToArray();
            }

            return base.Count(predicates);
        }
    }
}
