using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public class PlatformsFilter : IFilter<GameFilterEntity, Game>
    {
        public IQueryable<Game> Execute(GameFilterEntity filterModel, IQueryable<Game> input)
        {
            if (filterModel.Platforms != null && filterModel.Platforms.Any())
            {
                return input
                    .Where(game => game.GamePlatformTypes
                        .Any(platform => filterModel.Platforms
                            .Contains(platform.PlatformTypeId)));
            }

            return input;
        }
    }
}
