using System.ComponentModel.DataAnnotations;

namespace RestASPNet.Data.DTO.V1
{
    public class FileUploadDTO
    {
        [Required]
        public IFormFile File { get; set; }
    }
}
