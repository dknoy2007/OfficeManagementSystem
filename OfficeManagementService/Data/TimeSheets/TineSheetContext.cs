using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OfficeManagementService.Data.TimeSheets.Interfaces;
using OfficeManagementService.Models;

namespace OfficeManagementService.Data.TimeSheets
{
    public class TineSheetContext : ITineSheetContext
    {
        public TineSheetContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("TimeSheetsSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("TimeSheetsSettings:DatabaseName"));
            TimeSheets = database.GetCollection<Models.TimeSheet>(configuration.GetValue<string>("TimeSheetsSettings:CollectionName"));
        }

        public IMongoCollection<TimeSheet> TimeSheets { get; }
    }
}
