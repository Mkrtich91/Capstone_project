using MongoDB.Bson.Serialization.Serializers;
using MongoDB.Bson.Serialization;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Converter
{
    public class DateTimeConverter : SerializerBase<DateTime>
    {
        private const string DateFormat = "yyyy-MM-dd HH:mm:ss.fff";

        public override DateTime Deserialize(BsonDeserializationContext context, BsonDeserializationArgs args)
        {
            var bsonReader = context.Reader;

            switch (bsonReader.GetCurrentBsonType())
            {
                case BsonType.String:
                    var dateString = bsonReader.ReadString();
                    if (DateTime.TryParseExact(dateString, DateFormat, CultureInfo.InvariantCulture, DateTimeStyles.None, out var dateTime))
                    {
                        return dateTime;
                    }
                    throw new BsonSerializationException($"Cannot deserialize DateTime from string '{dateString}'.");

                case BsonType.Null:
                    bsonReader.ReadNull();
                    return default;

                default:
                    throw new BsonSerializationException($"Cannot deserialize DateTime of type '{bsonReader.GetCurrentBsonType()}'.");
            }
        }

        public override void Serialize(BsonSerializationContext context, BsonSerializationArgs args, DateTime value)
        {
            context.Writer.WriteString(value.ToString(DateFormat, CultureInfo.InvariantCulture));
        }
    }
}
