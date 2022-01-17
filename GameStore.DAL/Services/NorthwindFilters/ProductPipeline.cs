using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Services.Filters;

namespace GameStore.DAL.Services.NorthwindFilters
{
    public class ProductPipeline : Pipeline<GameFilterEntity, Product>
    {
        public override IQueryable<Product> Process(GameFilterEntity filterModel, IQueryable<Product> input)
        {
            Filters.ForEach(filter => input = filter.Execute(filterModel, input));
            TotalItems = input.Count();

            var result = filterModel.PageSize != (int)PageSize.All
                ? input.Skip((filterModel.PageNumber - 1) * filterModel.PageSize).Take(filterModel.PageSize)
                : input;

            return result;
        }
    }
}
