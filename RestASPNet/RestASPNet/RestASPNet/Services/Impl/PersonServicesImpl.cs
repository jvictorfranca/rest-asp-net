using RestASPNet.Controllers.Model;

namespace RestASPNet.Services.Impl
{
    public class PersonServicesImpl : IPersonServices
    {
        public List<Person> FindAll()
        {
            List<Person> persons = new List<Person>();
            for(int i = 0; i<8; i++)
            {
                persons.Add(MockPerson(i));
            }
            return persons;
        }

        public Person FindById(long id)
        {
            var person = MockPerson((int) id);

            return person;
        }

        private Person MockPerson(int i)
        {
            return new Person
            {
                Id = new Random().Next(1, 1000),
                FirstName = "Leandro " + i,
                LastName = "Costa " + i,
                Adress = "Rua dos Bobos, nº " + i,
                Gender = i % 2 == 0 ? "Male" : "Female"
            };
        }

        public Person Create(Person person)
        {
            return person;
        }
        public Person Update(Person person)
        {
            return person;
        }

        public void Delete(long id)
        {
            // No content
        }


    }
}
