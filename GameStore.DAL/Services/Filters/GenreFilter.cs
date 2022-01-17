using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;

namespace GameStore.DAL.Services.Filters
{
    public class GenreFilter : IFilter<GameFilterEntity, Game>
    {
        public IQueryable<Game> Execute(GameFilterEntity filterModel, IQueryable<Game> input)
        {
            if (filterModel.Genres != null && filterModel.Genres.Any())
            {
                return input
                    .Where(game => game.GameGenres
                        .Any(genre => filterModel.Genres
                            .Contains(genre.GenreId)));
            }

            return input;
        }
    }
}
