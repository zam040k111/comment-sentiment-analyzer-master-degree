using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Services.NorthwindFilters.BsonFilters
{
    public class SupplierBsonFilter : IFilterBson<GameFilterEntity>
    {
        public IAggregateFluent<BsonDocument> Execute(
            GameFilterEntity filterEntity,
            IAggregateFluent<BsonDocument> aggregate,
            IMongoCollection<BsonDocument> input)
        {
            var filter = filterEntity.Publishers != null && filterEntity.Publishers.Any()
                ? Builders<BsonDocument>.Filter.In("SupplierID", filterEntity.Publishers)
                : Builders<BsonDocument>.Filter.Empty;

            return aggregate.Match(filter);
        }
    }
}
