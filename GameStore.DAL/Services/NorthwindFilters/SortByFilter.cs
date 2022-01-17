using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Services.Filters;

namespace GameStore.DAL.Services.NorthwindFilters
{
    public class SortByFilter : IFilter<GameFilterEntity, Product>
    {
        public IQueryable<Product> Execute(GameFilterEntity filterModel, IQueryable<Product> input)
        {
            var methods = new Dictionary<SortBy, Expression<Func<Product, object>>>
            {
                //{SortBy.MostPopular, game => game.Viewed * -1},
                //{SortBy.MostCommented, game => game.Comments.Count * -1},
                {SortBy.PriceASC, game => game.Price},
                {SortBy.PriceDESC, game => game.Price * -1},
                //{SortBy.Published, game => game.Published.Ticks * -1}
            };

            return methods.ContainsKey(filterModel.SortBy) ? input.OrderBy(methods[filterModel.SortBy]) : input;
        }
    }
}
