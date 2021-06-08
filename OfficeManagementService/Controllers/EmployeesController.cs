using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeManagementService.Controllers.Utils;
using OfficeManagementService.Models;
using OfficeManagementService.Repositories.Employee.Interfaces;

namespace OfficeManagementService.Controllers
{
    [ApiController]
    [Route("api/Employee")]
    public class EmployeesController : ControllerBase
    {
        private readonly IEmployeeRepository _repository;

        public EmployeesController(IEmployeeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        [HttpGet("Seed")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> Seed(int numberOfEmployees)
        {
            var employees = await _repository.Seed(numberOfEmployees);
            return Ok(employees);
        }

        [HttpGet("Get")]
        [ProducesResponseType(typeof(IEnumerable<Employee>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<IEnumerable<Employee>>> GetEmployees()
        {
            var employees = await _repository.GetEmployees();
            return Ok(employees);
        }

        [HttpGet("{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(Employee), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Employee>> GetEmployee(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return BadRequest();
            }

            var employee = await _repository.GetEmployee(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(employee);
        }

        [HttpPost("Add")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> AddEmployee([FromBody] Employee employee)
        {
            if (!EmployeesControllerUtils.IsValidEmployee(employee))
            {
                return BadRequest();
            }

            return Ok(await _repository.AddEmployee(employee));
        }

        [HttpPut("Update")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> UpdateEmployee([FromBody] Employee employee)
        {
            if (!EmployeesControllerUtils.IsValidEmployee(employee))
            {
                return BadRequest();
            }

            return Ok(await _repository.UpdateEmployee(employee));
        }

        [HttpDelete("Delete/{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteEmployee(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return BadRequest();
            }

            return Ok(await _repository.DeleteEmployee(employeeId));
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAllEmployees()
        {
            return Ok(await _repository.DeleteAllEmployees());
        }
    }
}
