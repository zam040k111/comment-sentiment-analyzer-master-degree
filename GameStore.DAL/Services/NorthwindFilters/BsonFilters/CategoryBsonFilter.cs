using System;
using System.Linq;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Services.NorthwindFilters.BsonFilters
{
    public class CategoryBsonFilter : IFilterBson<GameFilterEntity>
    {
        public IAggregateFluent<BsonDocument> Execute(
            GameFilterEntity filterEntity,
            IAggregateFluent<BsonDocument> aggregate,
            IMongoCollection<BsonDocument> input)
        {
            var filter = filterEntity.Genres != null && filterEntity.Genres.Any()
                ? Builders<BsonDocument>.Filter.In("CategoryID", filterEntity.Genres)
                : Builders<BsonDocument>.Filter.Empty;
            
            return aggregate.Match(filter);
        }
    }
}
