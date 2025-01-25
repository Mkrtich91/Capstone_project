using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Entities
{
    public class Region
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("RegionID")]
        public int RegionID { get; set; }

        [BsonElement("RegionDescription")]
        public string RegionDescription { get; set; }
    }
}
