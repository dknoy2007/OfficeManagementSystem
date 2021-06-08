using System;
using System.Collections.Generic;
using MongoDB.Bson;

namespace OfficeManagementService.Repositories.Employee
{
    public static class EmployeeRepositoryUtils
    {
        private const string EmployeeFirstNamePrefix = "first";
        private const string EmployeeLastNamePrefix = "last";

        public static IEnumerable<Models.Employee> CreateSeedEmployees(int numberOfEmployees)
        {
            var employeeIds = new HashSet<string>();

            var employees = new List<Models.Employee>();

            for (var i = 1; i <= numberOfEmployees; i++)
            {
                employees.Add(CreateEmployee(i, employeeIds));
            }

            return employees;
        }

        private static Models.Employee CreateEmployee(int employeeNumber, ICollection<string> employeeIds)
        {
            var firstName = $"{EmployeeFirstNamePrefix}{employeeNumber}";
            var lastName = $"{EmployeeLastNamePrefix}{employeeNumber}";

            return new Models.Employee
            {
                Id = ObjectId.GenerateNewId().ToString(),
                EmployeeId = CreateEmployeeId(employeeIds),
                FirstName = $"{firstName}",
                LastName = $"{lastName}",
                EmailAddress = $"{firstName}.{lastName}@mail.com"
            };
        }

        private static string CreateEmployeeId(ICollection<string> employeeIds)
        {
            string employeeId;

            do
            {
                employeeId = new Random().Next(0, 1000000).ToString("D6");
            }
            while (employeeIds.Contains(employeeId));

            employeeIds.Add(employeeId);

            return employeeId;
        }
    }
}
