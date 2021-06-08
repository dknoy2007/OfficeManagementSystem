using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfficeManagementService.Models
{
    public class Employee
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("EmployeeId")]
        public string EmployeeId { get; set; }
        
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string EmailAddress { get; set; }
    }
}
