using System;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations.Serializers
{
    public class DateSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(DateTime);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var stringDate = context.Reader.ReadString();

            DateTime.TryParse(stringDate, out DateTime date);

            return date;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
        }
    }
}