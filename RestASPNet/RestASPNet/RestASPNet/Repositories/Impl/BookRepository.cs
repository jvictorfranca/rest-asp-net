using RestASPNet.Controllers.Model;
using RestASPNet.Controllers.Model.Context;

namespace RestASPNet.Repositories.Impl
{
    public class BookRepository : IBookRepository
    {
        private MSSQLContext _context;

        public BookRepository(MSSQLContext context)
        {
            _context = context;
        }

        public List<Book> FindAll()
        {


            return _context.Books.ToList();
        }

        public Book FindById(long id)
        {
            var Book = _context.Books.Find(id);

            return Book;
        }

        public Book Create(Book Book)
        {
            Book = _context.Add(Book).Entity;
            _context.SaveChanges();
            return Book;
        }
        public Book Update(Book Book)
        {
            var existingBook = _context.Books.Find(Book.Id);
            if (existingBook == null) return null;
            _context.Entry(existingBook).CurrentValues.SetValues(Book);
            _context.SaveChanges();
            return Book;
        }

        public void Delete(long id)
        {
            var existingBook = _context.Books.Find(id);
            if (existingBook == null) return;
            _context.Remove(existingBook);
            _context.SaveChanges();
        }
    }
}
