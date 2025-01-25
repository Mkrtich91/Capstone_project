using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Entities
{
    public class Shipper
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("ShipperID")]
        public int ShipperID { get; set; }

        [BsonElement("CompanyName")]
        public string CompanyName { get; set; }

        [BsonElement("Phone")]
        public string Phone { get; set; }
    }

}
