using Microsoft.AspNetCore.Mvc;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestLogsController : ControllerBase
    {
        private readonly ILogger _logger;

        public TestLogsController(ILogger<TestLogsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]

        public IActionResult LogTest()
        {
            _logger.LogDebug("this is a debug log");
            _logger.LogInformation("This is an information log.");
            _logger.LogWarning("This is a warning log.");
            _logger.LogError("This is an error log.");
            _logger.LogCritical("This is a critical log");
            return Ok("Logs have been generated. Check the console output.");
        }
    }
}
