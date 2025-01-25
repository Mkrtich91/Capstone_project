using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.IO;

namespace MongoDB.Entities.Converter
{
    public class PostalCodeConverter : SerializerBase<string>
    {
        public override string Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;

            switch (bsonReader.GetCurrentBsonType())
            {
                case BsonType.String:
                    return bsonReader.ReadString();
                case BsonType.Int32:
                    return bsonReader.ReadInt32().ToString();
                case BsonType.Null:
                    bsonReader.ReadNull();
                    return null;
                default:
                    throw new BsonSerializationException($"Cannot deserialize PostalCode of type '{bsonReader.GetCurrentBsonType()}'.");
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, string value)
        {
            if (value == null)
            {
                context.Writer.WriteNull();
                return;
            }

            context.Writer.WriteString(value);
        }
    }
}
