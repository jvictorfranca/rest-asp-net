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

        public byte[] GetFile(string fileName)
        {
            throw new NotImplementedException();
        }


        public Task<FileDetailDTO> SaveToDisk(IFormFile file)
        {
            throw new NotImplementedException();
        }
        public Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
