using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace MongoDB.Entities.Entities
{
    public class Category
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("CategoryID")]
        public int CategoryID { get; set; }

        [BsonElement("CategoryName")]
        public string CategoryName { get; set; }

        [BsonElement("Description")]
        public string Description { get; set; }

        [BsonElement("Picture")]
        public string Picture { get; set; }
    }
}
