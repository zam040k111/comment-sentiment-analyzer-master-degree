using System;
using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public class DateFilter : IFilter<GameFilterEntity, Game>
    {
        public IQueryable<Game> Execute(GameFilterEntity filterModel, IQueryable<Game> input)
        {
            if (filterModel.Published != TimeSpan.Zero)
            {
                var minDate = DateTime.Now - filterModel.Published;
                return input.Where(game => minDate < game.Published);
            }

            return input;
        }
    }
}
