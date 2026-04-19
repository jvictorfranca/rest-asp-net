using CsvHelper.Configuration;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Files.Exporters.Contract;
using System.Text;

namespace RestASPNet.Files.Exporters.Factory
{
    internal class CsvExporter : IFileExporter
    {
        public FileContentResult ExportFile(List<PersonDTO> people)
        {
            using var memoryStream = new MemoryStream();
            using var writer = new StreamWriter(memoryStream, Encoding.UTF8, leaveOpen:true);
            using var csv = new CsvHelper.CsvWriter
            (
                writer,
                new CsvConfiguration(System.Globalization.CultureInfo.InvariantCulture)
                {
                    HasHeaderRecord = true,
                }

            );
            csv.WriteRecords(people);
            writer.Flush();

            var fileBytes = memoryStream.ToArray();

            return new FileContentResult(fileBytes, MediaTypes.ApplicationCsv)
            {
                FileDownloadName = $"people_exported_{DateTime.UtcNow:yyyyMMddHHmmss}.csv"
            };
        }
    }
}