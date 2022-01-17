using System;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations.Serializers
{
    class StringToDecimalSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(decimal);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            if (context.Reader.CurrentBsonType == MongoDB.Bson.BsonType.String)
            {
                var value = context.Reader.ReadString();
                decimal.TryParse(value, out decimal decimalValue);

                return decimalValue;
            }

            return 0;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value) { }
    }
}
