using System;
using System.Linq;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace GameStore.DAL.Northwind.EntityConfigurations.Serializers
{
    public class ObjectIdSerializer : IBsonSerializer
    {
        public Type ValueType => typeof(Guid);

        public object Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var id = context.Reader.ReadObjectId().ToString();
            var stringGuid = new Guid(ObjectId.Parse(id).ToByteArray().Concat(new byte[] { 5, 5, 5, 5 }).ToArray());

            return stringGuid;
        }

        public void Serialize(BsonSerializationContext context, BsonSerializationArgs args, object value)
        {
        }
    }
}