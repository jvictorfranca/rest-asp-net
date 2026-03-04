using RestASPNet.Controllers.Model;
using RestASPNet.Controllers.Model.Context;

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
    };
}
