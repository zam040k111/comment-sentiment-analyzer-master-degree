using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind.Entities;

namespace GameStore.DAL.Services.NorthwindFilters
{
    public class PriceFilter : IFilter<GameFilterEntity, Product>
    {
        public IQueryable<Product> Execute(GameFilterEntity filterModel, IQueryable<Product> input)
        {
            if (filterModel.PriceFrom != 0.0M)
            {
                return input.Where(game => game.Price >= filterModel.PriceFrom);
            }

            if (filterModel.PriceTo != 0.0M)
            {
                return input.Where(game => game.Price >= filterModel.PriceFrom && game.Price <= filterModel.PriceTo);
            }

            return input;
        }
    }
}
