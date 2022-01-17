using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind;
using GameStore.DAL.Northwind.Entities;

namespace GameStore.DAL.Logger
{
    public class MongoLogger : IMongoLogger
    {
        private readonly NorthwindContext _mongoContext;

        public MongoLogger(NorthwindContext mongoContext)
        {
            _mongoContext = mongoContext;
        }

        public void AddLog(Log log)
        {
            _mongoContext.Logs.InsertOne(log);
        }
    }
}