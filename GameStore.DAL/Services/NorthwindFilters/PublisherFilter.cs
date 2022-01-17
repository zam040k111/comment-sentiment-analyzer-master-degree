using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind.Entities;

namespace GameStore.DAL.Services.NorthwindFilters
{
    public class SupllierFilter : IFilter<GameFilterEntity, Product>
    {
        public IQueryable<Product> Execute(GameFilterEntity filterModel, IQueryable<Product> input)
        {
            if (filterModel.Publishers != null && filterModel.Publishers.Any())
            {
                return input.Where(game => filterModel.Publishers.Contains(game.SupplierId));
            }

            return input;
        }
    }
}
