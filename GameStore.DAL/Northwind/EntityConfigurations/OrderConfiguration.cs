using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Northwind.EntityConfigurations.Serializers;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations
{
    public class OrderConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Orders>(cm =>
            {
                cm.AutoMap();

                cm.MapMember(c => c.DateTime).SetElementName("OrderDate").SetSerializer(new DateSerializer());
                cm.MapMember(c => c.CustomerId).SetElementName("CustomerID");
                cm.MapMember(c => c.OrderId).SetElementName("OrderID");
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}