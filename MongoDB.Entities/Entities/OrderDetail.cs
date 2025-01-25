using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MongoDB.Entities.Converter;

namespace MongoDB.Entities.Entities
{
    public class OrderDetail
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("OrderID")]
        public int OrderID { get; set; }

        [BsonElement("ProductID")]
        public int ProductID { get; set; }

        [BsonElement("UnitPrice")]
        public decimal UnitPrice { get; set; }

        [BsonElement("Quantity")]
        public int Quantity { get; set; }

        [BsonElement("Discount")]
        public decimal Discount { get; set; }
    }
}
