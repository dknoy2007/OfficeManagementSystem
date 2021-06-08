using System;

namespace OfficeManagementService.Models
{
    public class TimeSheetReport
    {
        public long Enter { get; set; }
        public long Exit { get; set; }

        public override string ToString()
        {
            var date = GetReportDateString();

            return string.IsNullOrWhiteSpace(date) 
                ? string.Empty 
                : $"{date} Enter: {ConvertTicksToTimeString(Enter)} , Exit: {ConvertTicksToTimeString(Exit)}";
        }

        private string GetReportDateString()
        {
            return Enter == 0 ? string.Empty : new DateTime(Enter).ToString("MM/dd/yyyy");
        }

        private static string ConvertTicksToTimeString(long ticks)
        {
            return ticks == 0 ? string.Empty : new DateTime(ticks).ToString("hh:mm tt");
        }
    }
}
