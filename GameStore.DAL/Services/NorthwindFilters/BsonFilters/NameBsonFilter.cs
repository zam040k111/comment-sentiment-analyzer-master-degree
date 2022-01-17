using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Services.NorthwindFilters.BsonFilters
{
    public class NameBsonFilter : IFilterBson<GameFilterEntity>
    {
        public IAggregateFluent<BsonDocument> Execute(
            GameFilterEntity filterEntity,
            IAggregateFluent<BsonDocument> aggregate,
            IMongoCollection<BsonDocument> input)
        {
            var filter = !string.IsNullOrWhiteSpace(filterEntity.Name)
                ? Builders<BsonDocument>.Filter.Regex("ProductName", new BsonRegularExpression(filterEntity.Name ?? ""))
                : Builders<BsonDocument>.Filter.Empty;

            return aggregate.Match(filter);
        }
    }
}
