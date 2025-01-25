using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MongoDB.Entities.Entities
{
    public class Employee
    {
        [BsonId]
        public ObjectId Id { get; set; }

        [BsonElement("EmployeeID")]
        public int EmployeeID { get; set; }

        [BsonElement("LastName")]
        public string LastName { get; set; }

        [BsonElement("FirstName")]
        public string FirstName { get; set; }

        [BsonElement("Title")]
        public string Title { get; set; }

        [BsonElement("TitleOfCourtesy")]
        public string TitleOfCourtesy { get; set; }

        [BsonElement("BirthDate")]
        public DateTime BirthDate { get; set; }

        [BsonElement("HireDate")]
        public DateTime HireDate { get; set; }

        [BsonElement("Address")]
        public string Address { get; set; }

        [BsonElement("City")]
        public string City { get; set; }

        [BsonElement("Region")]
        public string? Region { get; set; }

        [BsonElement("PostalCode")]
        public string PostalCode { get; set; }

        [BsonElement("Country")]
        public string Country { get; set; }

        [BsonElement("HomePhone")]
        public string HomePhone { get; set; }

        [BsonElement("Extension")]
        public string Extension { get; set; }

        [BsonElement("Photo")]
        public string Photo { get; set; }

        [BsonElement("Notes")]
        public string Notes { get; set; }

        [BsonElement("ReportsTo")]
        public string ReportsTo { get; set; }

        [BsonElement("PhotoPath")]
        public string? PhotoPath { get; set; }
    }
}
