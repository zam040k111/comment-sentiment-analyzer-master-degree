using MongoDB.Bson.Serialization.Attributes;

namespace GameStore.DAL.Northwind.Entities
{
    public class Shipper : MongoModel
    {
        [BsonElement("ShipperID")]
        public int ShipperId { get; set; }

        public string CompanyName { get; set; }

        public string Phone { get; set; }
    }
}