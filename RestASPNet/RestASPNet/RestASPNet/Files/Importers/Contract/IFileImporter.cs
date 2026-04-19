using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Files.Importers.Contract
{
    public interface IFileImporter
    {
        Task<List<PersonDTO>> ImportFileAsync<T>(Stream fileStream);
    }
}
