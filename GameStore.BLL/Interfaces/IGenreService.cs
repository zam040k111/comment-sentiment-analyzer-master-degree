using System.Collections.Generic;
using GameStore.BLL.DTO;

namespace GameStore.BLL.Interfaces
{
    public interface IGenreService : IValidatedService<GenreDto>
    {
        GenreDto GetByName(string name);
        List<GenreDto> GetByGame(int gameId);
    }
}
