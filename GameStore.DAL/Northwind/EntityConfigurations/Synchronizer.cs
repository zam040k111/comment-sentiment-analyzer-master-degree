using System.Collections.Generic;
using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.DAL.Northwind.Entities;

namespace GameStore.DAL.Northwind.EntityConfigurations
{
    public class Synchronizer<TEntity, TNorthwind>
        where TEntity : SoftDeletable
        where TNorthwind : MongoModel
    {
        public IEnumerable<TEntity> Include(IEnumerable<TNorthwind> input, NorthwindContext context, IMapper mapper)
        {
            if (typeof(TEntity) == typeof(Game) && typeof(TNorthwind) == typeof(Product))
            {
                return (IEnumerable<TEntity>)((IEnumerable<Product>) input).Include(context, mapper);
            }

            return null;
        }
    }
}
