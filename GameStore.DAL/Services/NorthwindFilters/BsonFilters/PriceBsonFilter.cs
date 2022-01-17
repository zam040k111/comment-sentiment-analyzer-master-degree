using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Services.NorthwindFilters.BsonFilters
{
    public class PriceBsonFilter : IFilterBson<GameFilterEntity>
    {
        public IAggregateFluent<BsonDocument> Execute(
            GameFilterEntity filterEntity,
            IAggregateFluent<BsonDocument> aggregate,
            IMongoCollection<BsonDocument> input)
        {
            if (filterEntity.PriceFrom == 0 && filterEntity.PriceTo == 0)
            {
                return aggregate;
            }

            var docs = input.Find(Builders<BsonDocument>.Filter.Type("UnitPrice", BsonType.String)).ToList();
            foreach (var item in docs)
            {
                var value = item.GetValue("UnitPrice");
                item.Set("UnitPrice", decimal.Parse(value.AsString));
                input.FindOneAndReplace(Builders<BsonDocument>.Filter.Eq("_id", item.GetValue("_id")), item);
            }

            var from = Builders<BsonDocument>.Filter.Gte("UnitPrice", (BsonDecimal128)filterEntity.PriceFrom);
            var to = Builders<BsonDocument>.Filter.Lte("UnitPrice", (BsonDecimal128)filterEntity.PriceTo);

            var filter = filterEntity.PriceTo > 0
                ? Builders<BsonDocument>.Filter.And(new[] { from, to })
                : from;

            return aggregate.Match(filter);
        }
    }
}
