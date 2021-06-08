using MongoDB.Driver;

namespace OfficeManagementService.Data.Employee.Interfaces
{
    public interface IEmployeeContext
    {
        IMongoCollection<Models.Employee> Employees { get; }
    }
}
