using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Files.Exporters.Contract
{
    public interface IFileExporter
    {
        FileContentResult ExportFile(List<PersonDTO> people);
    }
}
