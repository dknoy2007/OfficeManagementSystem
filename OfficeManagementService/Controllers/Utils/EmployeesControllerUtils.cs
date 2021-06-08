using System.Linq;
using MongoDB.Bson;
using OfficeManagementService.Models;

namespace OfficeManagementService.Controllers.Utils
{
    public static class EmployeesControllerUtils
    {
        private const int EmployIdLength = 6;
        public static bool IsValidEmployee(Employee employee)
        {
            return employee != null && 
                   !string.IsNullOrWhiteSpace(employee.Id) &&
                   !string.IsNullOrWhiteSpace(employee.EmployeeId) && 
                   ObjectId.TryParse(employee.Id, out _) &&
                   employee.EmployeeId.Length == EmployIdLength &&
                   employee.EmployeeId.All(char.IsDigit);
        }
    }
}
