using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Services;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookServices _BookServices;
        private readonly ILogger<BookController> _logger;

        public BookController(IBookServices bookServices, ILogger<BookController> logger)
        {
            _BookServices = bookServices;
            _logger = logger;
        }

        [HttpGet(Name = "GetAllBooks")]
        [ProducesResponseType(200, Type = typeof(List<BookDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all people");
            return Ok(_BookServices.FindAll());
        }

        [HttpGet("{id}", Name = "GetBookById")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Get(long id)
        {
            _logger.LogInformation("Fetching Book with ID {id}", id);
            var Book = _BookServices.FindById(id);
            if (Book == null)
            {
                _logger.LogWarning("Book with ID {id} not found", id);
                return NotFound();
            }
            return Ok(Book);

        }

        [HttpPost(Name = "CreateBook")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Post([FromBody] BookDTO book)
         {
            _logger.LogInformation("Creating new Book {title}", book.Title);
            var createdBook = _BookServices.Create(book);
            if (createdBook == null)
            {
                _logger.LogError("Failed to create Book {title}", book.Title);
                return NotFound();
            }
            return Ok(createdBook);
        }

        [HttpPut(Name = "UpdateBook")]
        [ProducesResponseType(200, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Put([FromBody] BookDTO book)
        {
            _logger.LogInformation("Updating Book with ID {id}", book.Id);
            var createdBook = _BookServices.Update(book);
            if (createdBook == null)
            {
                _logger.LogError("Book with ID {id} not found for update", book.Id);
                return NotFound();
            }
            _logger.LogDebug("Book with ID {id} updated successfully", book.Id);
            return Ok(createdBook);
        }

        [HttpDelete("{id}", Name = "DeleteBook")]
        [ProducesResponseType(204, Type = typeof(BookDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Fetching Book with ID {id}", id);
            _BookServices.Delete(id);
            return NoContent();
        }
        
    }
}