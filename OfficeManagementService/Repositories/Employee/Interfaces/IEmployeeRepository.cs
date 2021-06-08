using System.Collections.Generic;
using System.Threading.Tasks;

namespace OfficeManagementService.Repositories.Employee.Interfaces
{
    public interface IEmployeeRepository
    {
        Task<IEnumerable<Models.Employee>> Seed(int numberOfEmployees);
        Task<IEnumerable<Models.Employee>> GetEmployees();
        Task<Models.Employee> GetEmployee(string employeeId);
        Task<bool> AddEmployee(Models.Employee employee);
        Task<bool> UpdateEmployee(Models.Employee employee);
        Task<bool> DeleteEmployee(string employeeId);
        Task<bool> DeleteAllEmployees();
    }
}
