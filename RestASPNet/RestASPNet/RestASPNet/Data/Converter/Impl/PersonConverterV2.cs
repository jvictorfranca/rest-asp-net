using RestASPNet.Controllers.Model;
using RestASPNet.Data.Converter.Contract;
using RestASPNet.Data.DTO.V2;

namespace RestASPNet.Data.Converter.Impl
{
    public class PersonConverterV2 : IParser<Person, PersonDTO>, IParser<PersonDTO, Person>
    {
        public Person Parse(PersonDTO origin)
        {
            if (origin == null) return null;
            return new Person
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Adress = origin.Adress,
                Gender = origin.Gender
                //BirthDay = origin.BirthDay
            };
        }

        public List<Person> ParseList(List<PersonDTO> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }

        public PersonDTO Parse(Person origin)
        {
            if (origin == null) return null;
            return new PersonDTO
            {
                Id = origin.Id,
                FirstName = origin.FirstName,
                LastName = origin.LastName,
                Adress = origin.Adress,
                Gender = origin.Gender,
                BirthDay = DateTime.Now
                //BirthDay = origin.BirthDay ?? DateTime.Now,
            };
        }

        public List<PersonDTO> ParseList(List<Person> origin)
        {
            if (origin == null) return null;
            return origin.Select(item => Parse(item)).ToList();
        }
    }
}
