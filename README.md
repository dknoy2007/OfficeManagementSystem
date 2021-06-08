# OfficeManagementSystem

A .Net Core Web.API site (with Swagger UI) that Represents an Office system for users to:

* Manipulate employee data - Add employee/s, Get employee/s, update employee data, delete employee/s.
* Log employee date&time when arriving and leaving the office.
* Notify employees that were exposed to a Covid-19 positive employee that they should enter a quarantine.

## Endpoints:

### EmployeesController

* **api/Employee/Seed** - Create seed employees given number of employees to create.
* **api/Employee/Get**  - Get all employees from database.
* **api/Employee/{employeeId}** - Get employee given employeeId.
* **api/Employee/Add** - Add new employee.
* **api/Employee/Update** - Update existing employee.
* **api/Employee//Delete/{employeeId}** - Delete existing employee, given employeeId.
* **api/Employee/Delete** - Delete all employees.

### TimeSheetController

* **api/TimeSheet/Report/{employeeId}** - Log employee date&time when arriving and leaving the office, given employeeId.
* **api/TimeSheet/Reports/{employeeId}** - Get all the employee's time reports, in ascending order, given employeeId.

### Covid19Controller

* **api/Covid19Quarantine/Notify** - Send Covid-19 quarantine notification to employees that were exposed to an employee who was diagnosed as positive.

### HealthCheck

* **/health-check** - site health check
* **/health-check-details** - site health check details

## Technical 

* The site uses MongoDb as database.
* In order to run the mongoDb and/or site on docker locally, you must have docker desktop running locally.
* Run **docker pull mongo** and then **docker run --name aspnetrun-mongo -d -p 27017:27017 mongo** in order for mongo to run on docker locally (port 27017).
* When running the site locally, it's url is **localhost:5000**
* Running **docker-compose up** will create and run a composed image of the site and database on docker locally
