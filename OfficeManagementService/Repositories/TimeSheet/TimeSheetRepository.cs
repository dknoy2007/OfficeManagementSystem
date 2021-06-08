using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson;
using MongoDB.Driver;
using OfficeManagementService.Data.TimeSheets.Interfaces;
using OfficeManagementService.Models;
using OfficeManagementService.Repositories.TimeSheet.Interfaces;

namespace OfficeManagementService.Repositories.TimeSheet
{
    public class TimeSheetRepository : ITimeSheetRepository
    {
        private readonly ITineSheetContext _context;

        public TimeSheetRepository(ITineSheetContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<bool> Report(string employeeId)
        {
            var timeOfReport = DateTime.UtcNow;

            var timeSheet = await _context
                    .TimeSheets
                    .Find(p => p.EmployeeId == employeeId)
                    .FirstOrDefaultAsync();

            if (timeSheet == null)
            {
                return await AddNewEmployeeTimeSheet(employeeId, timeOfReport);
            }

            return await UpdateEmployeeTimeSheet(employeeId, timeOfReport, timeSheet);
        }

        public async Task<List<TimeSheetReport>> GetReports(string employeeId)
        {
            var timeSheet = await _context
                .TimeSheets
                .Find(p => p.EmployeeId == employeeId)
                .FirstOrDefaultAsync();

            return timeSheet?.Reports.Values
                .OrderBy(x => x.Enter)
                .ToList();
        }

        public async Task<bool> DeleteEmployeeReports(string employeeId)
        {
            var filter = Builders<Models.TimeSheet>.Filter.Eq(p => p.EmployeeId, employeeId);

            var deleteResult = await _context
                .TimeSheets
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteAllReports()
        {
            var deleteResult = await _context
                .TimeSheets
                .DeleteManyAsync(Builders<Models.TimeSheet>.Filter.Empty);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        private async Task<bool> AddNewEmployeeTimeSheet(string employeeId, DateTime timeOfReport)
        {
            var key = GetKey(timeOfReport);
            var id = ObjectId.GenerateNewId().ToString();
            var timeSheet = new Models.TimeSheet(id, employeeId);
            var timeSheetReport = new TimeSheetReport {Enter = timeOfReport.Ticks};

            if (!timeSheet.Reports.TryAdd(key, timeSheetReport))
            {
                return false;
            }

            await _context.TimeSheets.InsertOneAsync(timeSheet);
            return true;
        }

        private async Task<bool> UpdateEmployeeTimeSheet(string employeeId, DateTime timeOfReport, 
            Models.TimeSheet timeSheet)
        {
            var key = GetKey(timeOfReport);
            var reports = timeSheet.Reports;
            var timeOfReportInTicks = timeOfReport.Ticks;

            if (reports.ContainsKey(key))
            {
                var report = reports[key];

                if (report.Enter > timeOfReportInTicks || report.Exit > timeOfReportInTicks)
                {
                    return false;
                }

                report.Exit = timeOfReportInTicks;
            }
            else
            {
                var report = new TimeSheetReport {Enter = timeOfReport.Ticks};
                if (!reports.TryAdd(key, report))
                {
                    return false;
                }
            }

            var updateResult = await _context
                .TimeSheets
                .ReplaceOneAsync(filter: g => g.EmployeeId == employeeId, replacement: timeSheet);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        private static string GetKey(DateTime timeOfReport)
        {
            return $"{timeOfReport.Day}_{timeOfReport.Month}_{timeOfReport.Year}";
        }
    }
}
