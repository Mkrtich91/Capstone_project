using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Entities
{
    public class EntityChangeLog
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("Timestamp")]
        public DateTime Timestamp { get; set; }

        [BsonElement("Action")]
        public string Action { get; set; }

        [BsonElement("EntityType")]
        public string EntityType { get; set; }

        [BsonElement("OldVersion")]
        public string OldVersion { get; set; }

        [BsonElement("NewVersion")]
        public string NewVersion { get; set; }
    }
}
