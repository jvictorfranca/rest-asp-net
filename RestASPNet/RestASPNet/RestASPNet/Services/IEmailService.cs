using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Services
{
    public interface IEmailService
    {
        void SendSimpleEmail(string to, string subject, string body);
        Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment);


    }
}
