using System;
using System.Collections.Generic;
using GameStore.DAL.Northwind.Entities;
using GameStore.DAL.Northwind.EntityConfigurations;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind
{
    public static class NortwindConfig
    {
        public static Dictionary<Type, string> CollectionName = new Dictionary<Type, string>
        {
            {typeof(Orders), "orders"},
            {typeof(OrderDetails), "order-details"},
            {typeof(Category), "categories"},
            {typeof(Shipper), "shippers"},
            {typeof(Supplier), "suppliers.csv"},
            {typeof(Product), "products"},
            {typeof(Log), "logs"}
        };

        public static void Configure()
        {
            //BsonClassMap.RegisterClassMap<BaseEntity>(
            //    cm =>
            //    {
            //        cm.AutoMap();
            //        cm.MapIdMember(c => c.Id).SetSerializer(new ObjectIdSerializer());
            //        cm.SetIsRootClass(true);
            //    });

            BsonClassMap.RegisterClassMap<MongoModel>(
                cm =>
                {
                    cm.AutoMap();
                    cm.MapIdMember(c => c.Id);
                    cm.SetIsRootClass(true);
                });

            ProductConfiguration.Configure();
            CategoryConfiguration.Configure();
            SupplierConfiguration.Configure();
            OrderDetailsConfiguration.Configure();
            OrderConfiguration.Configure();
        }
    }
}
