using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeManagementService.Models;
using OfficeManagementService.Repositories.Employee.Interfaces;
using OfficeManagementService.Services.TimeSheet.Interfaces;

namespace OfficeManagementService.Controllers
{
    [ApiController]
    [Route("api/TimeSheet")]
    public class TimeSheetController : ControllerBase
    {
        private readonly ITimeSheetService _service;
        private readonly IEmployeeRepository _employeeRepository;

        public TimeSheetController(ITimeSheetService service, IEmployeeRepository employeeRepository)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _employeeRepository = employeeRepository ?? throw new ArgumentNullException(nameof(employeeRepository));
        }

        [HttpPost("Report/{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<Employee>> Report(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return BadRequest();
            }

            var employee = await _employeeRepository.GetEmployee(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            return Ok(await _service.Report(employeeId));
        }

        [HttpGet("Reports/{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType((int)HttpStatusCode.NotFound)]
        [ProducesResponseType(typeof(List<string>), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<List<string>>> GetReports(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return BadRequest();
            }

            var employee = await _employeeRepository.GetEmployee(employeeId);

            if (employee == null)
            {
                return NotFound();
            }

            var reports = await _service.GetReports(employeeId);

            if (reports == null || !reports.Any())
            {
                return NotFound();
            }

            return Ok(reports);
        }

        [HttpDelete("Delete/{employeeId}")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteEmployeeReports(string employeeId)
        {
            if (string.IsNullOrWhiteSpace(employeeId))
            {
                return BadRequest();
            }

            return Ok(await _service.DeleteEmployeeReports(employeeId));
        }

        [HttpDelete("Delete")]
        [ProducesResponseType(typeof(bool), (int)HttpStatusCode.OK)]
        public async Task<IActionResult> DeleteAllReports()
        {
            return Ok(await _service.DeleteAllReports());
        }
    }
}
