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
                _emailService.SendSimpleEmail(emailRequest);
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {to}", emailRequest.To);
                return StatusCode(500, "Failed to send email");
            }
        }

        [HttpPost("with-attachment", Name = "SendEmailWithAttachment")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(string))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]

        public async Task<IActionResult> SendEmailWithAttachment([FromForm] string emailRequest, [FromForm] FileUploadDTO input) 
        {

            EmailRequestDTO emailRequestDto = null;
            var options = new System.Text.Json.JsonSerializerOptions { PropertyNameCaseInsensitive = true };
            emailRequestDto = System.Text.Json.JsonSerializer.Deserialize<EmailRequestDTO>(emailRequest, options);

            if (emailRequestDto == null)
            {
                _logger.LogError("Invalid email request data for email to {to}", emailRequestDto.To);
                return BadRequest("Invalid email request data");
            }

            var attachment = input?.File;

            if (attachment == null || attachment.Length == 0)
            {
                _logger.LogWarning("No file uploaded for email to {to}", emailRequestDto.To);
                return BadRequest("No file uploaded");
            }
            


            _logger.LogInformation("Sending email to {to} with subject {subject} and attachment {FileName}", emailRequestDto.To, emailRequestDto.Subject, attachment?.FileName);
            
            try
            {
                await _emailService.SendEmailWithAttachment(emailRequestDto, attachment);
                return Ok("Email sent successfully");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {to} with attachment {FileName}", emailRequestDto.To, attachment?.FileName);
                return StatusCode(500, "Failed to send email");
            }
        }
    }
}
