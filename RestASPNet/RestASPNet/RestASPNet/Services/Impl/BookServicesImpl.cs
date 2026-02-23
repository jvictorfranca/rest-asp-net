using Mapster;
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

        public List<BookDTO> FindAll()
        {
       

            return _repository.FindAll().Adapt<List<BookDTO>>();
        }

        public BookDTO FindById(long id)
        {
            var book = _repository.FindById(id).Adapt<BookDTO>();

            return book;
        }

        public BookDTO Create(BookDTO book)
        {
            var entityCreate = book.Adapt<Book>();
            return _repository.Create(entityCreate).Adapt<BookDTO>();
        }
        public BookDTO Update(BookDTO book)
        {
            var entityCreate = book.Adapt<Book>();
            return _repository.Update(entityCreate).Adapt<BookDTO>();
        }

        public void Delete(long id)
        {
            _repository.Delete(id);
        }


    }
}
