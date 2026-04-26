using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Services;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    [Authorize("Bearer")]
    public class FileController : ControllerBase
    {
        private IFileServices _fileServices;
        private readonly ILogger _logger;
        public FileController(IFileServices fileServices, ILogger<FileController> logger)
        {
            _fileServices = fileServices;
            _logger = logger;
        }

        [HttpGet("downloadFile/{fileName}", Name = "DownloadFile")]
        [ProducesResponseType(200, Type = typeof(byte[]))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        [Produces("application/octet-stream")]

        public async Task<IActionResult> DownloadFile(string fileName)
        {
            _logger.LogInformation("Downloading file {fileName}", fileName);
            try
            {
            var buffer = _fileServices.GetFile(fileName);
            if (buffer == null)
                    return NotFound();
                _logger.LogInformation("File {fileName} downloaded successfully", fileName);
                var contentType = $"application/{Path.GetExtension(fileName).TrimStart(".")}";
                return File(buffer, contentType, fileName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading file {fileName}", fileName);
                return BadRequest(ex.Message);
            }
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
