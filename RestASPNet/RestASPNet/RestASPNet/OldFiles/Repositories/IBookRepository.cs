using RestASPNet.Model;

namespace RestASPNet.OldFiles.Repositories
{
    public interface IBookRepository_BeforeGeneric
    {

        Book Create(Book Book);
        Book FindById(long id);
        List<Book> FindAll();
        Book Update(Book Book);
        void Delete(long id);

    }
}
