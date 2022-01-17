using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public class SortByFilter : IFilter<GameFilterEntity, Game>
    {
        public IQueryable<Game> Execute(GameFilterEntity filterModel, IQueryable<Game> input)
        {
            var methods = new Dictionary<SortBy, Expression<Func<Game, decimal>>>
            {
                {SortBy.MostPopular, game => game.Viewed * -1},
                {SortBy.MostCommented, game => game.Comments.Count * -1},
                {SortBy.PriceASC, game => game.Price},
                {SortBy.PriceDESC, game => game.Price * -1},
                {SortBy.Published, game => game.Published.Ticks * -1},
                {SortBy.ReviewsASC, game => (decimal)game.Score},
                {SortBy.ReviewsDESC, game => (decimal)game.Score * -1}
            };

            return methods.ContainsKey(filterModel.SortBy) 
                ? input.OrderBy(methods[filterModel.SortBy]) 
                : input;
        }
    }
}
