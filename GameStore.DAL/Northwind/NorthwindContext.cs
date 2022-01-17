using GameStore.DAL.Northwind.Entities;
using MongoDB.Driver;

namespace GameStore.DAL.Northwind
{
    public class NorthwindContext
    {
        public readonly IMongoDatabase Db;

        public IMongoCollection<Orders> Orders =>
            Db.GetCollection<Orders>(NortwindConfig.CollectionName[typeof(Orders)]);

        public IMongoCollection<OrderDetails> OrderDetails =>
            Db.GetCollection<OrderDetails>(NortwindConfig.CollectionName[typeof(OrderDetails)]);

        public IMongoCollection<Category> Categories =>
            Db.GetCollection<Category>(NortwindConfig.CollectionName[typeof(Category)]);

        public IMongoCollection<Supplier> Suppliers =>
            Db.GetCollection<Supplier>(NortwindConfig.CollectionName[typeof(Supplier)]);

        public IMongoCollection<Product> Products =>
            Db.GetCollection<Product>(NortwindConfig.CollectionName[typeof(Product)]);

        public IMongoCollection<Shipper> Shippers =>
            Db.GetCollection<Shipper>(NortwindConfig.CollectionName[typeof(Shipper)]);

        public IMongoCollection<Log> Logs =>
            Db.GetCollection<Log>(NortwindConfig.CollectionName[typeof(Log)]);

        public NorthwindContext(string connectionString)
        {
            var databaseName = MongoUrl.Create(connectionString).DatabaseName;
            var client = new MongoClient(connectionString);
            Db = client.GetDatabase(databaseName);
            NortwindConfig.Configure();
        }
    }
}
