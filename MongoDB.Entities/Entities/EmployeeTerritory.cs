using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace MongoDB.Entities.Entities
{
    public class EmployeeTerritory
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("EmployeeID")]
        public int EmployeeID { get; set; }

        [BsonElement("TerritoryID")]
        public int TerritoryID { get; set; }
    }
}
