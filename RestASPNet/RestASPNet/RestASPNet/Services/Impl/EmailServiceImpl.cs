using RestASPNet.Data.DTO.V1;
using RestASPNet.Mail;

namespace RestASPNet.Services.Impl
{
    public class EmailServiceImpl : IEmailService
    {

        private readonly EmailSender _emailSender;
        private readonly ILogger<EmailServiceImpl> _logger;


        public EmailServiceImpl(EmailSender emailSender, ILogger<EmailServiceImpl> logger)
        {
            _emailSender = emailSender;
            _logger = logger;
        }

        public void SendSimpleEmail(string to, string subject, string body)
        {
            _emailSender.To(to)
                        .WithSubject(subject)
                        .WithMessage(body)
                        .Send();
        }
        public Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment)
        {
            throw new NotImplementedException();
        }
    }
}
