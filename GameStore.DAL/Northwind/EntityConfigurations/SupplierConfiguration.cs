using GameStore.DAL.Northwind.Entities;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations
{
    public class SupplierConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Supplier>(cm =>
            {
                cm.AutoMap();

                cm.MapMember(c => c.SupplierId).SetElementName("SupplierID");
                cm.MapMember(c => c.Description).SetElementName("ContactTitle");
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}