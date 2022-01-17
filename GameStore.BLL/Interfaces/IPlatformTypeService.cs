using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IPlatformTypeService : IValidatedService<PlatformTypeDto>
    {
        List<PlatformTypeDto> GetByGame(int gameId);
    }
}
