using RestASPNet.Hypermedia.Utils;
using RestASPNet.Model;
using RestASPNet.Model.Context;

namespace RestASPNet.Repositories.Impl
{
    public class PersonRepository (MSSQLContext context) : GenericRepository<Person>(context), IPersonRepository
    {
        public Person Disable(long id)
        {
            var person = _context.Persons.Find(id);
            if (person == null) return null;
            person.Enabled = false;
            _context.SaveChanges();
            return person;
        }

        public List<Person> FindByName(string firstName, string lastName)
        {
            var query = _context.Persons.AsQueryable();
            if(!string.IsNullOrEmpty(firstName))
            {
                query = query.Where(p => p.FirstName.Contains(firstName));
            }
            if (!string.IsNullOrEmpty(lastName))
            { 
                query = query.Where(p => p.LastName.Contains(lastName));
            }
            return query.ToList();
        }

        public PagedSearch<Person> FindWithPagedSearch(string name, string sortDirection, int pageSize, int page)
        {
            var queryBuilder = new QueryBuilders.PersonQueryBuilder();
            var (query, countQuery, sort, size, offset) = queryBuilder.BuildQueries(name, sortDirection, pageSize, page);
            var persons = base.FindWithPagedSearch(query);
            var totalResults = base.GetCount(countQuery);

            return new PagedSearch<Person>()
            {
                CurrentPage = page,
                PageSize = size,
                Filters = [],
                sortDirection = sort,
                sortFields = null,
                TotalResults = totalResults,
                List = persons
            };
        }
    };
}
