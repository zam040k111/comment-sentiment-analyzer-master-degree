using System.Collections.Generic;

namespace GameStore.BLL.DTO
{
    public class GenreDto
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentGenreId { get; set; }

        public GenreDto ParentGenre { get; set; }

        public List<int?> GameGenresId { get; set; }

        public List<GameGenreDto> GameGenres { get; set; }
    }
}
