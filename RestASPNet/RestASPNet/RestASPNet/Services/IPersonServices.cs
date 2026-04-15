using RestASPNet.Data.DTO.V1;
using RestASPNet.Hypermedia.Utils;

namespace RestASPNet.Services
{
    public interface IPersonServices
    {
        PersonDTO Create(PersonDTO person);
        PersonDTO FindById(long id);
        List<PersonDTO> FindAll();
        PersonDTO Update(PersonDTO person);
        void Delete(long id);

        PersonDTO Disable(long id);

        List<PersonDTO> FindByName(string firstName, string lastName);

        PagedSearchDTO<PersonDTO> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page);

        Task<List<PersonDTO>> MassCreationAsync<T>(IFormFile file);
    }
}
