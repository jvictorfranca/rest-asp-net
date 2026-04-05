using Mapster;
using RestASPNet.Controllers.Model;
using RestASPNet.Data.Converter.Impl;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Hypermedia.Utils;
using RestASPNet.Repositories;

// This implementation uses the manual converter, and not the mapster library, as in bookservices for example
namespace RestASPNet.Services.Impl
{
    public class PersonServicesImplV1 : IPersonServices
    {

        private readonly IPersonRepository _repository;
        private readonly PersonConverterV1 _converter;

        public PersonServicesImplV1(IPersonRepository repository)
        {
            _repository = repository;
            _converter = new PersonConverterV1();
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
            var (query, countQuery, sort, size, offset) = BuildQueries(name, sortDirection, pageSize, page);
            var persons = _repository.FindWithPagedSearch(query);
            var totalResults = _repository.GetCount(countQuery);

            return new PagedSearchDTO<PersonDTO>()
            {
                CurrentPage = page,
                PageSize = size,
                Filters = [],
                sortDirection = sort,
                sortFields = null,
                TotalResults = totalResults,
                List = _converter.ParseList(persons)
            };
        }

        private (string query,
            string countQuery,
            string sort,
            int size,
            int offset
            ) BuildQueries(string name, string sortDirection, int pageSize, int page)
        {
            page = Math.Max(page, 1);
            var offset = (page - 1) * pageSize;
            var size = pageSize < 1 ? 1 : pageSize;

            var sort = !string.IsNullOrEmpty(sortDirection) && !sortDirection.ToLower().Equals("desc") ? "asc" : "desc";

            var baseQuery = $"FROM person p WHERE 1=1 ";
            if (!string.IsNullOrEmpty(name))
            {
                baseQuery += $"AND (p.first_name LIKE '%{name}%' OR p.last_name LIKE '%{name}%') ";

            }
            var query = $@"
                SELECT * {baseQuery} 
                ORDER BY p.first_name {sort} 
                OFFSET {offset} ROWS 
                FETCH NEXT {size} ROWS ONLY
                ";

            var countQuery = $"SELECT COUNT(*) {baseQuery}";

            return (query, countQuery, sort, size, offset);
            
        }
    }
}
