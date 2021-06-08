using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using OfficeManagementService.Models;
using OfficeManagementService.Services.Covid19.Interfaces;

namespace OfficeManagementService.Controllers
{
    [Route("api/Covid19Quarantine")]
    [ApiController]
    public class Covid19Controller : ControllerBase
    {
        private readonly ICovid19Service _service;

        public Covid19Controller(ICovid19Service service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
        }

        [HttpPost("Notify")]
        [ProducesResponseType((int)HttpStatusCode.BadRequest)]
        [ProducesResponseType(typeof(string), (int)HttpStatusCode.OK)]
        public async Task<ActionResult<string>> Notify([FromBody] List<QuarantineEmployee> employees)
        {
            if (employees == null || !employees.Any())
            {
                return BadRequest();
            }

            var notificationReport = await _service.NotifyQuarantineEmployees(
                employees
                    .GroupBy(x => x.EmployeeId)
                    .Select(y => y.First())
                    .ToList());
            
            return Ok(notificationReport);
        }
    }
}
