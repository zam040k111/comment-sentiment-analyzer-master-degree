using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace GameStore.DAL.Extensions
{
    public static class RepositoryExtensions
    {
        public static IEnumerable<T> Except<T, TKey>(this IEnumerable<T> items, IEnumerable<T> other, Func<T, TKey> getKeyFunc)
        {
            return items
                .GroupJoin(other, getKeyFunc, getKeyFunc, (item, tempItems) => new { item, tempItems })
                .SelectMany(t => t.tempItems.DefaultIfEmpty(), (t, temp) => new { t, temp })
                .Where(t => ReferenceEquals(null, t.temp) || t.temp.Equals(default(T)))
                .Select(t => t.t.item);
        }

        public static Expression<Func<T, bool>> CombinePredicates<T>(this List<Expression<Func<T, bool>>> predicates)
        {
            if (!predicates.Any() || predicates.Any(i => i == null))
            {
                Expression<Func<T, bool>> alwaysTrue = x => true;

                return alwaysTrue;
            }

            Expression<Func<T, bool>> firstFilter = predicates.First();

            var body = firstFilter.Body;
            var param = firstFilter.Parameters.ToArray();

            foreach (var nextFilter in predicates.Skip(1))
            {
                var nextBody = Expression.Invoke(nextFilter, param);
                body = Expression.AndAlso(body, nextBody);
            }
            
            Expression<Func<T, bool>> result = Expression.Lambda<Func<T, bool>>(body, param);
            
            return result;
        }
    }
}
