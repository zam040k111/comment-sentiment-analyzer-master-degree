using System;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations.Serializers
{
    public class IntToShortSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(short);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == MongoDB.Bson.BsonType.Double)
            {
                var intValue = context.Reader.ReadDouble();
                short.TryParse(intValue.ToString(), out short shortValue);

                return shortValue;
            }

            if (context.Reader.CurrentBsonType == MongoDB.Bson.BsonType.Int32)
            {
                var intValue = context.Reader.ReadInt32();

                return (short)intValue;
            }

            return 0;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value) { }
    }
}