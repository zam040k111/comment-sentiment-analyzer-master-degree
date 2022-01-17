using AutoMapper;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Northwind;
using GameStore.DAL.Northwind.Entities;
using MongoDB.Driver;

namespace GameStore.DAL.Repositories
{
    public class MongoGenericRepository<T> :
        ReadOnlyMongoGenericRepository<T>,
        IWriteOnlyMongoGenericRepository<T> where T : MongoModel
    {
        public MongoGenericRepository(NorthwindContext context, IMapper mapper) : base(context, mapper) { }

        public void Add(T item) => Collection.InsertOne(item);

        public void Update(T item)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, item.Id);
            Collection.FindOneAndReplace(filter, item);
        }

        public void Delete(T item)
        {
            var filter = Builders<T>.Filter.Eq(doc => doc.Id, item.Id);
            Collection.FindOneAndDelete(filter);
        }
    }
}
