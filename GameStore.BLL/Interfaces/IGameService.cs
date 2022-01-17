using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IGameService : IValidatedService<GameDto>
    {
        GameFilterDto GetAll(int pageSize, int pageNumber, bool includeIsDeleted = false);
        GameDto GetByKey(string key);
        GameDto GetByKeyFromBothDb(string key);
        List<GameDto> GetByGenre(int genreId);
        List<GameDto> GetByPlatformType(int platformTypeId);
        List<GameDto> GetByPublisher(int publisherId);
        GameFilterDto ApplyFilter(GameFilterDto filterModel);
        GameFilterDto GetAllFromBothDb(int pageSize, int pageNumber, bool includeIsDeleted = false);
        GameFilterDto ApplyFilterBothBd(GameFilterDto filterModel);
        int Count();
        List<GameDto> GetBest(int count);
    }
}
