using RestASPNet.Controllers.Model;
using RestASPNet.Data.Converter.Impl;
using RestASPNet.Data.DTO.V2;
using RestASPNet.Repositories;
using RestASPNet.Repositories.Impl;


// This implementation uses the manual converter, and not the mapster library, as in bookservices for example
namespace RestASPNet.Services.Impl
{
    public class PersonServicesImplV2
    {

        private readonly IRepository<Person> _repository;
        private readonly PersonConverterV2 _converter;

        public PersonServicesImplV2(IRepository<Person> repository)
        {
            _repository = repository;
            _converter = new PersonConverterV2();
        }


        public PersonDTO Create(PersonDTO person)
        {   var personEntity = _converter.Parse(person);
            return  _converter.Parse(_repository.Create(personEntity));
        }

    }
}
