using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind.Entities;

namespace GameStore.DAL.Services.NorthwindFilters
{
    public class NameFilter : IFilter<GameFilterEntity, Product>
    {
        public IQueryable<Product> Execute(GameFilterEntity filterModel, IQueryable<Product> input)
        {
            if (!string.IsNullOrWhiteSpace(filterModel.Name))
            {
                return input.Where(game => game.Name.Contains(filterModel.Name));
            }

            return input;
        }
    }
}
