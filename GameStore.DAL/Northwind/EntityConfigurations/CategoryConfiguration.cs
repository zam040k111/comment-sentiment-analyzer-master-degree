using GameStore.DAL.Northwind.Entities;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations
{
    public class CategoryConfiguration
    {
        public static void Configure()
        {
            BsonClassMap.RegisterClassMap<Category>(cm =>
            {
                cm.AutoMap();

                cm.MapMember(c => c.Name).SetElementName("CategoryName");
                cm.MapMember(c => c.CategoryId).SetElementName("CategoryID");
                cm.SetIgnoreExtraElements(true);
            });
        }
    }
}