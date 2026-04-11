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

        [HttpPost("uploadMultipleFiles", Name = "UploadMultipleFiles")]
        [Consumes("multipart/form-data")]
        [ProducesResponseType(200, Type = typeof(List<FileDetailDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/json", "application/xml")]

        public async Task<IActionResult> UploadMultipleFiles([FromForm] MultipleFilesUploadDTO input)
        {
            var files = input.Files;
            _logger.LogInformation("Uploading multiple files {number}", files.Count);
            try
            {
                var filesDetail = await _fileServices.SaveFilesToDisk(files);
                _logger.LogInformation("Files uploaded successfully");
                return Ok(filesDetail);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading multiple files");
                return BadRequest(ex.Message);
            }
        }
    }
}
