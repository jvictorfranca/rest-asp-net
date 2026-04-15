using CsvHelper.Configuration;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Model.Files.Importers.Contract;
using System.Globalization;

namespace RestASPNet.Model.Files.Importers.Factory
{
    internal class CSVFileImporter : IFileImporter
    {
        public async Task<List<PersonDTO>> ImportFileAsync<T>(Stream fileStream)
        {
            using var reader = new StreamReader(fileStream);
            using var csv = new CsvHelper.CsvReader(reader, new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                HasHeaderRecord = true,
                TrimOptions = TrimOptions.Trim,
                IgnoreBlankLines = true,

            });

            var people = new List<PersonDTO>();
            await foreach (var record in csv.GetRecordsAsync<dynamic>())
            {
                var person = new PersonDTO
                {
                    Id = record.Id,
                    FirstName = record.first_name,
                    LastName = record.last_name,
                    Adress = record.adress,
                    Gender = record.gender,
                    Enabled = true,
                };
                people.Add(person);
            }
        
            return people;
        }
    }
}