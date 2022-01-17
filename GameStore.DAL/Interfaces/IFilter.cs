
using System.Linq;

namespace GameStore.DAL.Interfaces
{
    public interface IFilter<TFilter, TEntity>
    {
        IQueryable<TEntity> Execute(TFilter filterModel , IQueryable<TEntity> input);
    }
}
