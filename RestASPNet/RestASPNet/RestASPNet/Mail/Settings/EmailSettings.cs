using DocumentFormat.OpenXml.Office.Y2022.FeaturePropertyBag;

namespace RestASPNet.Mail.Settings
{
    public class EmailSettings
    {
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; };
        public string Username { get; set; } = string.Empty;
        public string Password { get; set; }
        public string From { get; set; } = string.Empty;
        public string Message { get; set; }
        public string Subject { get; set; }
        public bool Ssl { get; set; }
        public MailSettings Properties { get; set; } = new MailSettings();

    }
}
