using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Services;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    // [EnableCors("LocalPolicy")] // For global CORS
    public class EmailController : ControllerBase
    {
        private readonly IEmailService _emailService;
        private readonly ILogger<EmailController> _logger;

        public EmailController(IEmailService emailService, ILogger<EmailController> logger)
        {
            _emailService = emailService;
            _logger = logger;
        }

        [HttpPost(Name = "SendSimpleEmail")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]

        public IActionResult SendSimpleEmail([FromBody] EmailRequestDTO emailRequest)
        {
            _logger.LogInformation("Sending email to {to} with subject {subject}", emailRequest.To, emailRequest.Subject);
            try
            {
                _emailService.SendSimpleEmail(emailRequest.To, emailRequest.Subject, emailRequest.body);
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {to}", emailRequest.To);
                return StatusCode(500, "Failed to send email");
            }
        }
    }
}
