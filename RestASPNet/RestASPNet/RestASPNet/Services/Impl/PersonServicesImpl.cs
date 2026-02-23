using RestASPNet.Controllers.Model;
using RestASPNet.Data.Converter.Impl;
using RestASPNet.Repositories;
using RestASPNet.Repositories.Impl;


// This implementation uses the manual converter, and not the mapster library, as in bookservices for example
namespace RestASPNet.Services.Impl
{
    public class PersonServicesImpl : IPersonServices
    {

        private readonly IRepository<Person> _repository;
        private readonly PersonConverter _converter;

        public PersonServicesImpl(IRepository<Person> repository)
        {
            _repository = repository;
            _converter = new PersonConverter();
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


    }
}
