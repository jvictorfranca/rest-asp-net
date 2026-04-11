using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Services
{
    public interface IFileServices
    {
        byte[] GetFile(string fileName);
        Task<FileDetailDTO> SaveToDisk(IFormFile file);
        Task<List<FileDetailDTO>> SaveFilesToDisk(List<IFormFile> files);
    }
}
