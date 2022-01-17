using System;
using System.Collections.Generic;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Services.Filters;
using MongoDB.Bson;
using MongoDB.Driver;

namespace GameStore.DAL.Services.NorthwindFilters.BsonFilters
{
    public class SortBsonFilter : IFilterBson<GameFilterEntity>
    {
        public IAggregateFluent<BsonDocument> Execute(
            GameFilterEntity filterEntity,
            IAggregateFluent<BsonDocument> aggregate,
            IMongoCollection<BsonDocument> input)
        {
            var methods = new Dictionary<SortBy, SortDefinition<BsonDocument>>
            {
                {SortBy.PriceDESC, Builders<BsonDocument>.Sort.Descending("UnitPrice")},
                {SortBy.PriceASC, Builders<BsonDocument>.Sort.Ascending("UnitPrice")}
            };

            var sortBy = methods.ContainsKey(filterEntity.SortBy)
                ? methods[filterEntity.SortBy]
                : null;

            return sortBy != null ? aggregate.Sort(sortBy) : aggregate;
        }
    }
}
