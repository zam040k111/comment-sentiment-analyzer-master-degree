using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public class NameFilter : IFilter<GameFilterEntity, Game>
    {
        public IQueryable<Game> Execute(GameFilterEntity filterModel, IQueryable<Game> input)
        {
            if (!string.IsNullOrWhiteSpace(filterModel.Name))
            {
                return input.Where(game => game.Name.Contains(filterModel.Name));
            }

            return input;
        }
    }
}
