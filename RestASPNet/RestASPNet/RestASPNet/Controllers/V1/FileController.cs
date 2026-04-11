using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Services;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class FileController : ControllerBase
    {
        private IFileServices _fileServices;
        private readonly ILogger _logger;
        public FileController(IFileServices fileServices, ILogger<FileController> logger)
        {
            _fileServices = fileServices;
            _logger = logger;
        }

        [HttpPost("uploadFile", Name = "UploadFile")]
        [ProducesResponseType(200, Type = typeof(FileDetailDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json", "application/xml")]

        public async Task<IActionResult> UploadFile( [FromForm] FileUploadDTO input)
        {
            var file = input.File;
            _logger.LogInformation("Uploading file {fileName}", file.FileName);
            try
            {
                var fileDetail = await _fileServices.SaveToDisk(file);
                _logger.LogInformation("File {fileName} uploaded successfully", file.FileName);
                return Ok(fileDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {fileName}", file.FileName);
                return BadRequest(ex.Message);
            }
        }
     }
}
