using System.Collections.Concurrent;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace OfficeManagementService.Models
{
    public class TimeSheet
    {
        public TimeSheet()
        {
            Reports = new ConcurrentDictionary<string, TimeSheetReport>();
        }

        public TimeSheet(string id, string employeeId) : this()
        {
            Id = id;
            EmployeeId = employeeId;
        }

        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string Id { get; set; }

        [BsonElement("EmployeeId")]
        public string EmployeeId { get; set; }

        public ConcurrentDictionary<string, TimeSheetReport> Reports { get; set; }
    }
}
