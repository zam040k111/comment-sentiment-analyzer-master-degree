using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Northwind.EntityConfigurations.Serializers;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations
{
    public class OrderDetailsConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<OrderDetails>(cm =>
            {
                cm.AutoMap();

                cm.MapMember(c => c.Discount).SetElementName("Discount").SetSerializer(new IntToFloatSerializer());
                cm.MapMember(c => c.Price).SetElementName("UnitPrice");
                cm.MapMember(c => c.OrderId).SetElementName("OrderID");
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}