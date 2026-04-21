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

        public void SendSimpleEmail(EmailRequestDTO emailRequest)
        {
            _emailSender.To(emailRequest.To)
                        .WithSubject(emailRequest.Subject)
                        .WithMessage(emailRequest.Body)
                        .Send();
        }
        public async Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment)
        {
            if (attachment == null || attachment.Length == 0) { 
            _logger.LogWarning("No attachment provided for email to {To}", emailRequest.To);
                throw new ArgumentException("Attachment is null or empty");
            }

            string tempFilePath = Path.Combine(Path.GetTempPath(), attachment.FileName);

            try
            {
                await using (var stream = new FileStream(tempFilePath, FileMode.Create))
                {
                    await attachment.CopyToAsync(stream);
                }
                    _emailSender.To(emailRequest.To)
                                .WithSubject(emailRequest.Subject)
                                .WithMessage(emailRequest.Body)
                                .Attach(tempFilePath)
                                .Send();
            }
            catch (Exception ex) 
            { 
            _logger.LogError(ex, "Failed to send email to {To} with attachment {FileName}", emailRequest.To, attachment.FileName);
                throw;
            }
            finally
            {
                File.Delete(tempFilePath);
            }

        }
    }
}
