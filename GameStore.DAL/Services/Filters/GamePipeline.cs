using System.Linq;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Services.Filters
{
    public class GamePipeline : Pipeline<GameFilterEntity, Game>
    {
        public override IQueryable<Game> Process(GameFilterEntity filterModel, IQueryable<Game> input)
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
