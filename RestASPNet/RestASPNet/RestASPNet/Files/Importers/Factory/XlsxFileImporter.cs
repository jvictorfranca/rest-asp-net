using ClosedXML.Excel;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Files.Importers.Contract;

namespace RestASPNet.Files.Importers.Factory
{
    internal class XlsxFileImporter : IFileImporter
    {
        public Task<List<PersonDTO>> ImportFileAsync<T>(Stream fileStream)
        {
            var people = new List<PersonDTO>();
            using var workbook = new XLWorkbook(fileStream);
            var worksheet = workbook.Worksheets.First();

            var rows = worksheet.RowsUsed().Skip(1);

            foreach (var row in rows)
            {
                if (!row.Cell(1).IsEmpty())
                {
                    var person = new PersonDTO
                    {
                        FirstName = row.Cell(1).GetValue<string>(),
                        LastName = row.Cell(2).GetValue<string>(),
                        Adress = row.Cell(3).GetValue<string>(),
                        Gender = row.Cell(4).GetValue<string>(),
                        Enabled = true
                    };
                    people.Add(person);
                } 
            }

            return Task.FromResult(people);

        }
    }
}