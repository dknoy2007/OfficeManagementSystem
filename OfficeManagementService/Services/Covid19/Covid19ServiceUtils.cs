using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Net;
using System.Net.Mail;
using OfficeManagementService.Models.Enums;

namespace OfficeManagementService.Services.Covid19
{
    public static class Covid19ServiceUtils
    {
        public static bool IsValidEmail(string email)
        {
            return !string.IsNullOrWhiteSpace(email) && new EmailAddressAttribute().IsValid(email);
        }

        public static bool SendSmtpMail(bool sendMail, string smtpServer, int port, string from, string to, 
            string subject, string body, int timeout = 100)
        {
            try
            {
                if (!sendMail)
                {
                    return true;
                }

                var message = new MailMessage(from, to, subject, body);
                var client = new SmtpClient(smtpServer)
                {
                    Port = port,
                    Timeout = timeout, 
                    Credentials = CredentialCache.DefaultNetworkCredentials
                };
                client.Send(message);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public static string ConvertQuarantineNotificationStatusToString(QuarantineNotificationStatus status)
        {
            return status switch
            {
                QuarantineNotificationStatus.EmployeeNotExist => "Employee not exist",
                QuarantineNotificationStatus.MissingMailAddress => "Missing email address",
                QuarantineNotificationStatus.InvalidMailAddress => "Invalid email address",
                QuarantineNotificationStatus.InvalidDateOfExposure => "Invalid date of exposure",
                QuarantineNotificationStatus.MailFailure => "Failed to send mail",
                QuarantineNotificationStatus.MailSent => "Quarantine notification was successfully sent",
                _ => throw new InvalidEnumArgumentException(
                    $"Invalid QuarantineNotificationStatus enum argument {status}")
            };
        }
    }
}
