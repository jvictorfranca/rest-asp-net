using RestASPNet.Controllers.Model;
using RestASPNet.Repositories;
using RestASPNet.Repositories.Impl;

namespace RestASPNet.Services.Impl
{
    public class PersonServicesImpl : IPersonServices
    {

        private readonly IPersonRepository _repository;

        public PersonServicesImpl(IPersonRepository repository)
        {
            _repository = repository;
        }

        public List<Person> FindAll()
        {
       

            return _repository.FindAll();
        }

        public Person FindById(long id)
        {
            var person = _repository.FindById(id);

            return person;
        }

        public Person Create(Person person)
        {
            return _repository.Create(person);
        }
        public Person Update(Person person)
        {
            
            return _repository.Update(person);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }


    }
}
