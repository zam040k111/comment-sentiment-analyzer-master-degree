using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Northwind.Entities
{
    public class MongoModel
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }
    }
}