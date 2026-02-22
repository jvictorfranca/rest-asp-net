using RestASPNet.Controllers.Model;
using RestASPNet.Controllers.Model.Context;

namespace RestASPNet.OldFiles.Repositories.Impl
{
    public class BookRepository_BeforeGeneric : IBookRepository_BeforeGeneric
    {
        private MSSQLContext _context;

        public BookRepository_BeforeGeneric(MSSQLContext context)
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

        public Book Create(Book book)
        {
            book = _context.Add(book).Entity;
            _context.SaveChanges();
            return book;
        }
        public Book Update(Book book)
        {
            var existingBook = _context.Books.Find(book.Id);
            if (existingBook == null) return null;
            _context.Entry(existingBook).CurrentValues.SetValues(book);
            _context.SaveChanges();
            return book;
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
