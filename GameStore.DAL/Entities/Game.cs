using System;
using System.Collections.Generic;

namespace GameStore.DAL.Entities
{
    public class Game : SoftDeletable
    {
        public int Id { get; set; }

        public string Key { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public int? PublisherId { get; set; }

        public DateTime Published { get; set; }

        public int Viewed { get; set; }

        public Publisher Publisher { get; set; }

        public byte[] Image { get; set; }

        public byte[] SmallImage { get; set; }

        public float Score { get; set; }

        public ICollection<GameGenre> GameGenres { get; set; }

        public ICollection<GamePlatformType> GamePlatformTypes { get; set; }

        public ICollection<Comment> Comments { get; set; }

        public Game()
        {
            GameGenres = new List<GameGenre>();
            GamePlatformTypes = new List<GamePlatformType>();
            Comments = new List<Comment>();
        }
    }
}
