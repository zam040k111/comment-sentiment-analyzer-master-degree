using System.Collections.Generic;
using GameStore.DAL.Entities;

namespace GameStore.DAL.Northwind.Entities
{
    public class Category : MongoModel
    {
        public int CategoryId { get; set; }

        public string Name { get; set; }

        public ICollection<GameGenre> GameGenres { get; set; }
    }
}