using System.Collections.Generic;
using System.Threading.Tasks;
using OfficeManagementService.Models;

namespace OfficeManagementService.Services.Covid19.Interfaces
{
    public interface ICovid19Service
    {
        Task<string> NotifyQuarantineEmployees(IEnumerable<QuarantineEmployee> quarantineEmployees);
    }
}
