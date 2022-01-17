using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using GameStore.DAL.Entities;
using GameStore.DAL.Northwind.Entities;
using MongoDB.Driver;

namespace GameStore.DAL.Northwind.EntityConfigurations
{
    public static class IncludeConfig
    {
        public static IEnumerable<Game> Include(this IEnumerable<Product> input, NorthwindContext context, IMapper mapper)
        {
            var games = mapper.Map<List<Game>>(input);
            foreach (var game in games)
            {
                var supplier = context.Suppliers.Find(i => i.SupplierId == game.PublisherId.Value)
                    .FirstOrDefault();
                game.Publisher = mapper.Map<Publisher>(supplier);

                var category = context.Categories.Find(i => i.CategoryId == game.GameGenres.FirstOrDefault().GenreId)
                    .FirstOrDefault();

                var gameGenre = game.GameGenres.FirstOrDefault();

                if (gameGenre != null)
                {
                    gameGenre.Genre = mapper.Map<Genre>(category);
                }
            }

            return games;
        }
    }
}
