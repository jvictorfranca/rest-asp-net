using RestASPNet.Data.DTO.V1;

namespace RestASPNet.Services
{
    public interface IBookServices
    {
        BookDTO Create(BookDTO book);
        BookDTO FindById(long id);
        List<BookDTO> FindAll();
        BookDTO Update(BookDTO book);
        void Delete(long id);
    }
}
