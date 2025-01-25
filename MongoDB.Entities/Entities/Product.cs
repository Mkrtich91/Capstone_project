using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Entities
{
    public class Product
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("ProductID")]
        public int ProductID { get; set; }

        [BsonElement("ProductName")]
        public string ProductName { get; set; }

        [BsonElement("SupplierID")]
        public int SupplierID { get; set; }

        [BsonElement("CategoryID")]
        public int CategoryID { get; set; }

        [BsonElement("QuantityPerUnit")]
        public string QuantityPerUnit { get; set; }

        [BsonElement("UnitPrice")]
        public decimal UnitPrice { get; set; }

        [BsonElement("UnitsInStock")]
        public int UnitsInStock { get; set; }

        [BsonElement("UnitsOnOrder")]
        public int UnitsOnOrder { get; set; }

        [BsonElement("ReorderLevel")]
        public int ReorderLevel { get; set; }

        [BsonElement("Discontinued")]
        public bool Discontinued { get; set; }
        [BsonElement("GameKey")]
        public string GameKey { get; set; }

        [BsonElement("ViewCount")]
        public int ViewCount { get; set; }
    }
}
