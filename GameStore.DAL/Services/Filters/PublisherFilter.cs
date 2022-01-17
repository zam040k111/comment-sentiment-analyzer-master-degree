using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public class PublisherFilter : IFilter<GameFilterEntity, Game>
    {
        public IQueryable<Game> Execute(GameFilterEntity filterModel, IQueryable<Game> input)
        {
            if (filterModel.Publishers != null && filterModel.Publishers.Any())
            {
                return input
                    .Where(game => filterModel.Publishers
                        .Contains(game.PublisherId));
            }

            return input;
        }
    }
}
