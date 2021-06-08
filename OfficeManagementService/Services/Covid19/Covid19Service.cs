using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using OfficeManagementService.Models;
using OfficeManagementService.Models.Enums;
using OfficeManagementService.Repositories.Employee.Interfaces;
using OfficeManagementService.Services.Covid19.Interfaces;

namespace OfficeManagementService.Services.Covid19
{
    public class Covid19Service : ICovid19Service
    {
        private readonly IEmployeeRepository _repository;
        private readonly IConfiguration _configuration;

        public Covid19Service(IConfiguration configuration, IEmployeeRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        public async Task<string> NotifyQuarantineEmployees(IEnumerable<QuarantineEmployee> quarantineEmployees)
        {
            var results = new ConcurrentBag<QuarantineNotificationResult>();

            var smtpMailInfo = GetSmtpMailInfoFromConfig();

            var tasks = 
                quarantineEmployees
                    .Select(employee => Task.Run(async () =>
                    {
                        var result = await SendQuarantineNotification(employee, smtpMailInfo);
                        results.Add(result);
                    }))
                    .ToList();

            await Task.WhenAll(tasks);

            return GetQuarantineNotificationResults(results);
        }

        private async Task<QuarantineNotificationResult> SendQuarantineNotification(
            QuarantineEmployee quarantineEmployee, SmtpMailInfo smtpMailInfo)
        {
            var result = new QuarantineNotificationResult(quarantineEmployee.EmployeeId, 
                quarantineEmployee.DateOfExposure);

            var employee = await _repository.GetEmployee(quarantineEmployee.EmployeeId);

            if (employee == null)
            {
                result.Status = QuarantineNotificationStatus.EmployeeNotExist;
                return result;
            }

            if (string.IsNullOrWhiteSpace(employee.EmailAddress))
            {
                result.Status = QuarantineNotificationStatus.MissingMailAddress;
                return result;
            }

            result.EmployeeEmailAddress = employee.EmailAddress;

            if (!Covid19ServiceUtils.IsValidEmail(employee.EmailAddress))
            {
                result.Status = QuarantineNotificationStatus.InvalidMailAddress;
                return result;
            }

            var now = DateTime.UtcNow;

            if (now < quarantineEmployee.DateOfExposure)
            {
                result.Status = QuarantineNotificationStatus.InvalidDateOfExposure;
                return result;
            }

            SendNotificationMail(quarantineEmployee, employee, smtpMailInfo, result);

            return result;
        }

        private void SendNotificationMail(QuarantineEmployee quarantineEmployee, Employee employee,
            SmtpMailInfo smtpMailInfo, QuarantineNotificationResult result)
        {
            var mailBody = GetNotificationMessage(smtpMailInfo.Body, quarantineEmployee.DateOfExposure);

            var mailStatus = Covid19ServiceUtils.SendSmtpMail(smtpMailInfo.SendMail,
                smtpMailInfo.SmtpServer, smtpMailInfo.SmtpPort, smtpMailInfo.From, 
                employee.EmailAddress, smtpMailInfo.Subject, mailBody);

            result.Status = mailStatus 
                ? QuarantineNotificationStatus.MailSent 
                : QuarantineNotificationStatus.MailFailure;
        }

        private string GetNotificationMessage(string mailBody, DateTime dateOfExposure)
        {
            var numberOfDaysInQuarantine = _configuration.GetValue<int>("Covid19QuarantineSettings:QuarantineNumberOfDays");
            var untilWhenInQuarantine = dateOfExposure.AddDays(numberOfDaysInQuarantine);
            
            return string.Format(mailBody, dateOfExposure.ToString("MM/dd/yyyy"),
                untilWhenInQuarantine.ToString("MM/dd/yyyy"));
        }
            

        private static string GetQuarantineNotificationResults(ConcurrentBag<QuarantineNotificationResult> results)
        {
            var sb = new StringBuilder();

            Parallel.ForEach(results, result =>
            {
                sb.Append(result);
                sb.AppendLine();
            });

            return sb.ToString();
        }

        private SmtpMailInfo GetSmtpMailInfoFromConfig()
        {
            return new SmtpMailInfo
            {
                SendMail = _configuration.GetValue<bool>("SmtpMailSettings:SendMail"),
                SmtpServer = _configuration.GetValue<string>("SmtpMailSettings:Server"),
                SmtpPort = _configuration.GetValue<int>("SmtpMailSettings:Port"),
                From = _configuration.GetValue<string>("SmtpMailSettings:OfficeMailAddress"),
                Subject = _configuration.GetValue<string>("Covid19QuarantineSettings:QuarantineEmailSubject"),
                Body = _configuration.GetValue<string>("Covid19QuarantineSettings:QuarantineEmailMessage")
            };
        }
    }
}
