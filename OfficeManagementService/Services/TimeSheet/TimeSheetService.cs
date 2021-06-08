using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OfficeManagementService.Repositories.Employee.Interfaces;
using OfficeManagementService.Repositories.TimeSheet.Interfaces;
using OfficeManagementService.Services.TimeSheet.Interfaces;

namespace OfficeManagementService.Services.TimeSheet
{
    public class TimeSheetService : ITimeSheetService
    {
        private readonly ITimeSheetRepository _timeSheetRepository;
        
        public TimeSheetService(ITimeSheetRepository timeSheetRepository, IEmployeeRepository employeeRepository)
        {
            _timeSheetRepository = timeSheetRepository ?? throw new ArgumentNullException(nameof(timeSheetRepository));
        }

        public async Task<bool> Report(string employeeId)
        {
            return await _timeSheetRepository.Report(employeeId);
        }

        public async Task<List<string>> GetReports(string employeeId)
        {
            var reports = await _timeSheetRepository.GetReports(employeeId);

            if (reports == null || !reports.Any())
            {
                return null;
            }

            return (from report in reports.AsParallel().AsOrdered()
                    let reportAsString = report.ToString()
                    where !string.IsNullOrWhiteSpace(reportAsString)
                    select reportAsString).ToList();
        }

        public async Task<bool> DeleteEmployeeReports(string employeeId)
        {
            return await _timeSheetRepository.DeleteEmployeeReports(employeeId);
        }

        public async Task<bool> DeleteAllReports()
        {
            return await _timeSheetRepository.DeleteAllReports();
        }
    }
}
