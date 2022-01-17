using System.Collections.Generic;
using GameStore.DAL.Entities;
using GameStore.DAL.Interfaces;
using GameStore.DAL.Services.Filters;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;

namespace GameStore.DAL.Services.NorthwindFilters.BsonFilters
{
    public class PipelineBson<TEntity>
    {
        protected readonly List<IFilterBson<GameFilterEntity>> Filters = new List<IFilterBson<GameFilterEntity>>();
        public int TotalItems { get; set; }

        public PipelineBson<TEntity> Register(IFilterBson<GameFilterEntity> filter)
        {
            Filters.Add(filter);

            return this;
        }

        public virtual List<TEntity> Process(IMongoCollection<BsonDocument> input, GameFilterEntity filter)
        {
            var aggregate = input.Aggregate();
            Filters.ForEach(i => aggregate = i.Execute(filter, aggregate, input));
            TotalItems = aggregate.ToList().Count;
            var bsonList = filter.PageSize != (int)PageSize.All
                ? aggregate.Skip((filter.PageNumber - 1) * filter.PageSize).Limit(filter.PageSize).ToList()
                : aggregate.ToList();

            var result = new List<TEntity>();
            bsonList.ForEach(i => result.Add(BsonSerializer.Deserialize<TEntity>(i)));

            return result;
        }
    }
}
