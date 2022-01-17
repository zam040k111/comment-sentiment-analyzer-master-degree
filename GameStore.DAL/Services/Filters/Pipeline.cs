using System.Collections.Generic;
using System.Linq;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public abstract class Pipeline<TFilter, TEntity>
    {
        protected readonly List<IFilter<TFilter, TEntity>> Filters = new List<IFilter<TFilter, TEntity>>();
        public int TotalItems { get; set; }

        public Pipeline<TFilter, TEntity> Register(IFilter<TFilter, TEntity> filter)
        {
            Filters.Add(filter);

            return this;
        }

        public abstract IQueryable<TEntity> Process(TFilter filterModel, IQueryable<TEntity> input);
    }
}
