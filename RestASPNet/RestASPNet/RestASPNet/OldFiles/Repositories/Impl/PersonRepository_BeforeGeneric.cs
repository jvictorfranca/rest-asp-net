namespace RestASPNet.OldFiles.Repositories.Impl
{
    using global::RestASPNet.Controllers.Model;
    using global::RestASPNet.Controllers.Model.Context;
    using global::RestASPNet.OldFiles.Repositories.RestASPNet.Repositories;

    namespace RestASPNet.Repositories.Impl
    {
        public class PersonRepository_BeforeGeneric : IPersonRepository_BeforeGeneric
        {
            private MSSQLContext _context;

            public PersonRepository_BeforeGeneric(MSSQLContext context)
            {
                _context = context;
            }

            public List<Person> FindAll()
            {


                return _context.Persons.ToList();
            }

            public Person FindById(long id)
            {
                var person = _context.Persons.Find(id);

                return person;
            }

            public Person Create(Person person)
            {
                person = _context.Add(person).Entity;
                _context.SaveChanges();
                return person;
            }
            public Person Update(Person person)
            {
                var existingPerson = _context.Persons.Find(person.Id);
                if (existingPerson == null) return null;
                _context.Entry(existingPerson).CurrentValues.SetValues(person);
                _context.SaveChanges();
                return person;
            }

            public void Delete(long id)
            {
                var existingPerson = _context.Persons.Find(id);
                if (existingPerson == null) return;
                _context.Remove(existingPerson);
                _context.SaveChanges();
            }
        }
    }

}
