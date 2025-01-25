using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;
using MongoDB.Entities.Converter;

namespace MongoDB.Entities.Entities
{
    public class Order
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("OrderID")]
        public int OrderID { get; set; }

        [BsonElement("CustomerID")]
        public string CustomerID { get; set; }

        [BsonElement("EmployeeID")]
        public int EmployeeID { get; set; }

        [BsonElement("OrderDate")]
        [BsonSerializer(typeof(DateTimeConverter))]
        public DateTime OrderDate { get; set; }

        [BsonElement("RequiredDate")]
        [BsonSerializer(typeof(DateTimeConverter))]
        public DateTime RequiredDate { get; set; }

        [BsonElement("ShippedDate")]
        [BsonSerializer(typeof(DateTimeConverter))]
        public DateTime ShippedDate { get; set; }
        
        [BsonElement("ShipVia")]
        public int ShipVia { get; set; }

        [BsonElement("Freight")]
        public decimal Freight { get; set; }

        [BsonElement("ShipName")]
        public string ShipName { get; set; }

        [BsonElement("ShipAddress")]
        public string ShipAddress { get; set; }

        [BsonElement("ShipCity")]
        public string ShipCity { get; set; }

        [BsonElement("ShipRegion")]
        public string? ShipRegion { get; set; }

        [BsonElement("ShipPostalCode")]
        public string ShipPostalCode { get; set; }

        [BsonElement("ShipCountry")]
        public string ShipCountry { get; set; }
    }

}

