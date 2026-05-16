namespace _66SMS.Infrastructure.Mails
{
    public class MailSetting
    {
        public const string SectionName = "MailSettings";
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; }
        public string User { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string FromName { get; set; } = string.Empty;
        public string FromEmail { get; set; } = string.Empty;
    }
}
