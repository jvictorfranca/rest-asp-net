using MailKit.Net.Smtp;
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

        public void Send()
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_settings.From, _settings.Username));
            message.To.AddRange(_recipients);
            message.Subject = _subject ?? _settings.Subject ?? "No subject";

            var builder = new BodyBuilder 
            { 
                TextBody = _body ?? _settings.Message ?? "" 
            };

            if(!string.IsNullOrEmpty(_attachment))
            {
                var filename = Path.GetFileName(_attachment);
                builder.Attachments.Add(filename, File.ReadAllBytes(_attachment));
            }

            message.Body = builder.ToMessageBody();

            try
            {
                using var client = new SmtpClient();
                client.Connect
                (
                    _settings.Host,
                    _settings.Port, 
                    _settings.Ssl ? MailKit.Security.SecureSocketOptions.StartTls : MailKit.Security.SecureSocketOptions.None
                );

                client.Authenticate(_settings.Username, _settings.Password);
                client.Send(message);


                _logger.LogInformation("Email sent successfully to {Recipients}", string.Join(";", _recipients));

            }
            catch (Exception ex) 
            {
                _logger.LogError(ex, "Failed to send email to {Recipients}", string.Join(";", _recipients));
            }

            finally
            {
                Reset();
            }

        }


        private IEnumerable<MailboxAddress> ParseRecipients(string to)
        {
            // email1@gmail.com ; email2@gmail.co
            var withoutSpaces = to.Replace(" ", "");

            var recipients = withoutSpaces.Split(';', StringSplitOptions.RemoveEmptyEntries);

            var list = new List<MailboxAddress>();

            foreach(var recipient in recipients)
            {
                try
                {
                    var mailbox = MailboxAddress.Parse(recipient);
                    list.Add(mailbox);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Invalid email address: {Recipient}", recipient);
                }
            }

            return list;
        }
        private void Reset()
        {
            _to = null;
            _subject = null;
            _recipients.Clear();
            _attachment = null;
            _body = null;
        }
    }
}
