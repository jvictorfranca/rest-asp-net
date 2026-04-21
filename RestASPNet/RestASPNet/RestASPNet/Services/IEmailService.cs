using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Services
{
    public interface IEmailService
    {
        void SendSimpleEmail(EmailRequestDTO emailRequest);
        Task SendEmailWithAttachment(EmailRequestDTO emailRequest, IFormFile attachment);


    }
}
