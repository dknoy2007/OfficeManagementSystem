using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using OfficeManagementService.Data.Employee.Interfaces;
using OfficeManagementService.Repositories.Employee.Interfaces;

namespace OfficeManagementService.Repositories.Employee
{
    public class EmployeeRepository : IEmployeeRepository
    {
        private readonly IEmployeeContext _context;

        public EmployeeRepository(IEmployeeContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<Models.Employee>> Seed(int numberOfEmployees)
        {
            if (numberOfEmployees == 0)
            {
                return null;
            }

            var seed = EmployeeRepositoryUtils.CreateSeedEmployees(numberOfEmployees);

            await _context.Employees.InsertManyAsync(seed);

            return await GetEmployees();
        }

        public async Task<IEnumerable<Models.Employee>> GetEmployees()
        {
            return await _context
                .Employees
                .Find(p => true)
                .ToListAsync();
        }

        public async Task<Models.Employee> GetEmployee(string employeeId)
        {
            return await _context
                .Employees
                .Find(p => p.EmployeeId == employeeId)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> AddEmployee(Models.Employee employee)
        {
            var isExist = await IsExist(employee);

            if (isExist)
            {
                return false;
            }

            await _context.Employees.InsertOneAsync(employee);

            return true;
        }

        public async Task<bool> UpdateEmployee(Models.Employee employee)
        {
            var updateResult = await _context
                .Employees
                .ReplaceOneAsync(filter: g => g.EmployeeId == employee.EmployeeId, replacement: employee);

            return updateResult.IsAcknowledged && updateResult.ModifiedCount > 0;
        }

        public  async Task<bool> DeleteEmployee(string employeeId)
        {
            var filter = Builders<Models.Employee>.Filter.Eq(p => p.EmployeeId, employeeId);

            var deleteResult = await _context
                .Employees
                .DeleteOneAsync(filter);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        public async Task<bool> DeleteAllEmployees()
        {
            var deleteResult = await _context
                .Employees
                .DeleteManyAsync(Builders<Models.Employee>.Filter.Empty);

            return deleteResult.IsAcknowledged && deleteResult.DeletedCount > 0;
        }

        private async Task<bool> IsExist(Models.Employee employee)
        {
            var existing = await _context
                .Employees
                .Find(p => p.Id == employee.Id || p.EmployeeId == employee.EmployeeId)
                .FirstOrDefaultAsync();

            return existing != null;
        }
    }
}
