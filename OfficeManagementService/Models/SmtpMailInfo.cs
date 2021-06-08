namespace OfficeManagementService.Models
{
    public class SmtpMailInfo
    {
        public bool SendMail { get; set; }
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
    }
}
