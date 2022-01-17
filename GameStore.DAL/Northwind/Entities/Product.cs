using System.Collections.Generic;
using GameStore.DAL.Entities;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Northwind.Entities
{
    public class Product : MongoModel
    {
        public string Key { get; set; }
        
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public short UnitsInStock { get; set; }

        public bool Discontinued { get; set; }

        public int ProductId { get; set; }

        public int CategoryId { get; set; }

        public int SupplierId { get; set; }

        [BsonIgnore]
        public ICollection<GameGenre> GameGenres { get; set; }
    }
}