using RestASPNet.Controllers.Model;
using RestASPNet.OldFiles.Repositories.RestASPNet.Repositories;
using RestASPNet.Services;

namespace RestASPNet.OldFiles.Services.Impl
{

    namespace RestASPNet.Services.Impl
    {
        public class PersonServicesImpl_BeforeGeneric : IPersonServices
        {

            private readonly IPersonRepository_BeforeGeneric _repository;

            public PersonServicesImpl_BeforeGeneric(IPersonRepository_BeforeGeneric repository)
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

            public PersonDTO Create(PersonDTO person)
            {
                throw new NotImplementedException();
            }

            PersonDTO IPersonServices.FindById(long id)
            {
                throw new NotImplementedException();
            }

            List<PersonDTO> IPersonServices.FindAll()
            {
                throw new NotImplementedException();
            }

            public PersonDTO Update(PersonDTO person)
            {
                throw new NotImplementedException();
            }
        }
    }
}
