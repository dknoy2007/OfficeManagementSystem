using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeManagementService.Models;

namespace OfficeManagementService.Repositories.TimeSheet.Interfaces
{
    public interface ITimeSheetRepository
    {
        Task<bool> Report(string employeeId);
        Task<List<TimeSheetReport>> GetReports(string employeeId);
        Task<bool> DeleteEmployeeReports(string employeeId);
        Task<bool> DeleteAllReports();
    }
}
