using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Interfaces
{
    public interface IFilterBson<TFilterEntity>
    {
        IAggregateFluent<BsonDocument> Execute(
            TFilterEntity filterEntity,
            IAggregateFluent<BsonDocument> aggregate,
            IMongoCollection<BsonDocument> input);
    }
}
