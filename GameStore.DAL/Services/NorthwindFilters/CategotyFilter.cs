using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind.Entities;

namespace GameStore.DAL.Services.NorthwindFilters
{
    public class CategoryFilter : IFilter<GameFilterEntity, Product>
    {
        public IQueryable<Product> Execute(GameFilterEntity filterModel, IQueryable<Product> input)
        {
            if (filterModel.Genres != null && filterModel.Genres.Any())
            {
                return input.Where(game => filterModel.Genres.Contains(game.CategoryId));
            }

            return input;
        }
    }
}
