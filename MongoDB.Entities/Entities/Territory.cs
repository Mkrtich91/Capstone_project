using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDB.Entities.Entities
{
    public class Territory
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("TerritoryID")]
        public int TerritoryID { get; set; }

        [BsonElement("TerritoryDescription")]
        public string TerritoryDescription { get; set; }

        [BsonElement("RegionID")]
        public int RegionID { get; set; }
    }
}
