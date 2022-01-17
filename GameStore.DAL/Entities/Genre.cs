using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class Genre : SoftDeletable
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int? ParentGenreId { get; set; }

        public Genre ParentGenre { get; set; }

        public ICollection<GameGenre> GameGenres { get; set; }

        public Genre()
        {
            GameGenres = new List<GameGenre>();
        }
    }
}
