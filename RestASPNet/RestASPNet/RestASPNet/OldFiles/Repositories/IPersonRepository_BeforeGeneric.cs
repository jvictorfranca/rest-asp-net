namespace RestASPNet.OldFiles.Repositories
{
    using global::RestASPNet.Controllers.Model;
    namespace RestASPNet.Repositories
    {
        public interface IPersonRepository_BeforeGeneric
        {

            Person Create(Person Person);
            Person FindById(long id);
            List<Person> FindAll();
            Person Update(Person Person);
            void Delete(long id);

        }
    }

}
