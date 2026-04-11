using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Services.Impl
{
    public class FileServicesImpl : IFileServices
    {
        private readonly string _basePath;
        private readonly IHttpContextAccessor _context;

        private static readonly HashSet<string> _allowedExtensions = new() { ".txt", ".pdf", ".docx", ".xlsx", ".jpg", ".jpeg", ".png", ".gif"};

        public FileServicesImpl(IConfiguration configuration, IHttpContextAccessor context)
        {
            _context = context;
            _basePath = Path.Combine(Directory.GetCurrentDirectory(), "UploadedDir");
            if(!Directory.Exists(_basePath))
            {
                Directory.CreateDirectory(_basePath);
            }
        }
        public async Task<FileDetailDTO> SaveToDisk(IFormFile file)
        {
            if (file == null || file.Length == 0)
                throw new Exception("File is empty");

            var fileExtension = Path.GetExtension(file.FileName).ToLower();
            if(!_allowedExtensions.Contains(fileExtension))
                throw new Exception("File type not allowed");

            var documentName = Path.GetFileName(file.FileName);

            var destination = Path.Combine(_basePath, documentName);

            var baseUrl = $"{_context.HttpContext.Request.Scheme}://{_context.HttpContext.Request.Host}";

            var fileDetail = new FileDetailDTO
            {
                DocumentName = documentName,
                DocType = fileExtension,
                DocUrl = $"{baseUrl}/api/file/v1/downloadFile/{documentName}"
            };

            using var stream = new FileStream(destination, FileMode.Create);

            await file.CopyToAsync(stream);

            return fileDetail;


        }

        public byte[] GetFile(string fileName)
        {
            throw new NotImplementedException();
        }


        public Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
