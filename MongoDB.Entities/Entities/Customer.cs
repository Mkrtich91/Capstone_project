using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Entities
{
    public class Customer
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("CustomerID")]
        public string CustomerID { get; set; }

        [BsonElement("CompanyName")]
        public string CompanyName { get; set; }

        [BsonElement("ContactName")]
        public string ContactName { get; set; }

        [BsonElement("ContactTitle")]
        public string ContactTitle { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("City")]
        public string City { get; set; }

        [BsonElement("Region")]
        public string? Region { get; set; }

        [BsonElement("PostalCode")]
        public string? PostalCode { get; set; }

        [BsonElement("Country")]
        public string Country { get; set; }

        [BsonElement("Phone")]
        public string Phone { get; set; }

        [BsonElement("Fax")]
        public string? Fax { get; set; }
    }
}
