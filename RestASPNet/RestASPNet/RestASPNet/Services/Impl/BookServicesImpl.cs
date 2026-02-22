using RestASPNet.Controllers.Model;
using RestASPNet.OldFiles.Repositories;
using RestASPNet.Repositories;
using RestASPNet.Repositories.Impl;

namespace RestASPNet.Services.Impl
{
    public class BookServicesImpl : IBookServices
    {

        private readonly IRepository<Book> _repository;

        public BookServicesImpl(IRepository<Book> repository)
        {
            _repository = repository;
        }

        public List<Book> FindAll()
        {
       

            return _repository.FindAll();
        }

        public Book FindById(long id)
        {
            var Book = _repository.FindById(id);

            return Book;
        }

        public Book Create(Book Book)
        {
            return _repository.Create(Book);
        }
        public Book Update(Book Book)
        {
            
            return _repository.Update(Book);
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }


    }
}
