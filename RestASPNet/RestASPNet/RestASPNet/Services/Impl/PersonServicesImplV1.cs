using Mapster;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Controllers.Model;
using RestASPNet.Data.Converter.Impl;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Files.Exporters.Factory;
using RestASPNet.Files.Importers.Factory;
using RestASPNet.Hypermedia.Utils;
using RestASPNet.Model;
using RestASPNet.Repositories;

// This implementation uses the manual converter, and not the mapster library, as in bookservices for example
namespace RestASPNet.Services.Impl
{
    public class PersonServicesImplV1 : IPersonServices
    {

        private readonly IPersonRepository _repository;
        private readonly PersonConverterV1 _converter;
        private readonly FileImporterFactory _fileImportFactory;
        private readonly FileExporterFactory _fileExporterFactory;
        private readonly ILogger<PersonServicesImplV1> _logger;

        public PersonServicesImplV1(IPersonRepository repository, FileImporterFactory fileImporterFactory, FileExporterFactory fileExporterFactory, ILogger<PersonServicesImplV1> logger)
        {
            _repository = repository;
            _converter = new PersonConverterV1();
            _fileImportFactory = fileImporterFactory;
            _fileExporterFactory = fileExporterFactory;
            _logger = logger;
        }

        public List<PersonDTO> FindAll()
        {
       

            return _converter.ParseList(_repository.FindAll());
        }

        public PersonDTO FindById(long id)
        {
            var person = _repository.FindById(id);

            return _converter.Parse(person);
        }

        public PersonDTO Create(PersonDTO person)
        {   var personEntity = _converter.Parse(person);
            return  _converter.Parse(_repository.Create(personEntity));
        }
        public PersonDTO Update(PersonDTO person)
        {
            var personEntity = _converter.Parse(person);
            return _converter.Parse(_repository.Update(personEntity));
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }

        public PersonDTO Disable(long id)
        {
           var entity = _repository.Disable(id);
            return entity?.Adapt<PersonDTO>();
        }

        public List<PersonDTO> FindByName(string firstName, string lastName)
        {
            return _converter.ParseList(_repository.FindByName(firstName, lastName));
        }

        public PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            
            var result = _repository.FindWithPagedSearch(name, sortDirection, pageSize, page);

            return result.Adapt<PagedSearchDTO<PersonDTO>>();
        }

        public async Task<List<PersonDTO>> MassCreationAsync<T>(IFormFile file)
        {
            if(file == null)
            {
                _logger.LogError("File is null. Cannot proceed with mass creation.");
                throw new ArgumentNullException("file");
            }
            using var stream = file.OpenReadStream();
            var filename = file.FileName;

            try
            {
                var importer = _fileImportFactory.GetFileImporter(filename);
                var persons = await importer.ImportFileAsync<List<PersonDTO>>(stream); // Consider using await instead of .Result for better async handling

                var entities = persons.Select(dto => _repository.Create(dto.Adapt<Person>())).ToList();

                return entities.Adapt<List<PersonDTO>>();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred during mass creation from file.");
                throw; // Re-throw the exception after logging it

            }
        }

        public FileContentResult ExportPage
        (
            int page,
            int pageSize,
            string sortDirection,
            string acceptHeader,
            string name
        )
        {
            _logger.LogInformation("Exporting page {Page} with page size {PageSize}, sort direction {SortDirection}, accept header {AcceptHeader}, and name filter {Name}", page, pageSize, sortDirection, acceptHeader, name);
            var content = FindWithPagedSearch(name, sortDirection, pageSize, page);


            try
            {

                var exporter = _fileExporterFactory.GetExporter(acceptHeader);

                var people = content.List.Adapt<List<PersonDTO>>();

                return exporter.ExportFile(people);
            }

            catch (Exception ex) 
            {
                _logger.LogError(ex, "An error occurred during file export.");
                throw;
            }


        }
    }
}
