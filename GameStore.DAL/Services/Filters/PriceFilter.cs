using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public class PriceFilter : IFilter<GameFilterEntity, Game>
    {
        public IQueryable<Game> Execute(GameFilterEntity filterModel, IQueryable<Game> input)
        {
            return filterModel.PriceTo > 0
                ? input.Where(game => game.Price >= filterModel.PriceFrom && game.Price <= filterModel.PriceTo)
                : input.Where(game => game.Price >= filterModel.PriceFrom);
        }
    }
}
