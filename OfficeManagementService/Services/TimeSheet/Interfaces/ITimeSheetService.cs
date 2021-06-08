using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfficeManagementService.Services.TimeSheet.Interfaces
{
    public interface ITimeSheetService
    {
        Task<bool> Report(string employeeId);
        Task<List<string>> GetReports(string employeeId);
        Task<bool> DeleteEmployeeReports(string employeeId);
        Task<bool> DeleteAllReports();
    }
}
