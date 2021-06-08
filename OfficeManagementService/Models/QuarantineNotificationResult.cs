using System;
using OfficeManagementService.Models.Enums;
using OfficeManagementService.Services.Covid19;

namespace OfficeManagementService.Models
{
    public class QuarantineNotificationResult
    {
        public QuarantineNotificationResult()
        {

        }

        public QuarantineNotificationResult(string employeeId, DateTime dateOfExposure)
        {
            EmployeeId = employeeId;
            DateOfExposure = dateOfExposure;
        }

        public string EmployeeId { get; set; }
        public string EmployeeEmailAddress { get; set; }
        public DateTime DateOfExposure { get; set; }
        public QuarantineNotificationStatus Status { get; set; }

        public override string ToString()
        {
            var status = Covid19ServiceUtils.ConvertQuarantineNotificationStatusToString(Status);
            return $"Id: {EmployeeId}, Date of Exposure: {DateOfExposure:MM/dd/yyyy}, Email: {EmployeeEmailAddress}, Status: {status}";
        }
    }
}
