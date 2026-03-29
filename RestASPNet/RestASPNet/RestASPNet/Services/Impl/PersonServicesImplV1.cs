using Mapster;
using RestASPNet.Controllers.Model;
using RestASPNet.Data.Converter.Impl;
using RestASPNet.Data.DTO.V1;
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
    }
}
