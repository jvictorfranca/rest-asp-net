using RestASPNet.Controllers.Model;

namespace RestASPNet.Repositories
{
    public interface IPersonRepository
    {

        Person Create(Person Person);
        Person FindById(long id);
        List<Person> FindAll();
        Person Update(Person Person);
        void Delete(long id);

    }
}
