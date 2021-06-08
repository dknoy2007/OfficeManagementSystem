using MongoDB.Driver;

namespace OfficeManagementService.Data.TimeSheets.Interfaces
{
    public interface ITineSheetContext
    {
        IMongoCollection<Models.TimeSheet> TimeSheets { get; }
    }
}
