using System;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations.Serializers
{
    public class IntToFloatSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(float);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == MongoDB.Bson.BsonType.Double)
            {
                var intValue = context.Reader.ReadDouble();
                float.TryParse(intValue.ToString(), out float shortValue);

                return shortValue;
            }

            if (context.Reader.CurrentBsonType == MongoDB.Bson.BsonType.Int32)
            {
                var intValue = context.Reader.ReadInt32();

                return (float)intValue;
            }

            return 0;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value) { }
    }
}