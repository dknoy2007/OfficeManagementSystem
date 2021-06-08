using Microsoft.Extensions.Configuration;
using MongoDB.Driver;
using OfficeManagementService.Data.Employee.Interfaces;

namespace OfficeManagementService.Data.Employee
{
    public class EmployeeContext : IEmployeeContext
    {
        public EmployeeContext(IConfiguration configuration)
        {
            var client = new MongoClient(configuration.GetValue<string>("EmployeesSettings:ConnectionString"));
            var database = client.GetDatabase(configuration.GetValue<string>("EmployeesSettings:DatabaseName"));
            Employees = database.GetCollection<Models.Employee>(configuration.GetValue<string>("EmployeesSettings:CollectionName"));
        }

        public IMongoCollection<Models.Employee> Employees { get; }
    }
}
