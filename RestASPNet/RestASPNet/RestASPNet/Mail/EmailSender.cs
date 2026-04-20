using MimeKit;
using RestASPNet.Mail.Settings;

namespace RestASPNet.Mail
{
    public class EmailSender
    {
        private readonly EmailSettings _settings;
        private readonly ILogger<EmailSender> _logger;

        private string _to;
        private string _subject;
        private string _body;

        private readonly List<MailboxAddress> _recipients = new();

        private string? _attachment;

        public EmailSender(EmailSettings settings, ILogger<EmailSender> logger)
        {
            _settings = settings;
            _logger = logger;
        }

        public EmailSender To(string to)
        {
            _to = to;
            _recipients.Clear();
            _recipients.Add(ParseRecipients(to));
            return this;
        }

        public EmailSender WithSubject(string subject)
        {
            _subject = subject;
            return this;
        }

        public EmailSender WithMessage(string body)
        {
            _body = body;
            return this;
        }

        public EmailSender Attachment(string filePath)
        {
            if(File.Exists(filePath))
            {
                _attachment = filePath;

            }
            else
            {
                _logger.LogError("Attachment file not found: {FilePath}", filePath);
            }
            return this;
        }

        private MailboxAddress ParseRecipients(string to)
        {
            throw new NotImplementedException();
        }
    }
}
