using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Controllers.Model;
using RestASPNet.Services;

namespace RestASPNet.Controllers
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

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all people");
            return Ok(_BookServices.FindAll());
        }

        [HttpGet("{id}")]
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

        [HttpPost]
        public IActionResult Post([FromBody] Book Book)
         {
            _logger.LogInformation("Creating new Book {title}", Book.Title);
            var createdBook = _BookServices.Create(Book);
            if (createdBook == null)
            {
                _logger.LogError("Failed to create Book {title}", Book.Title);
                return NotFound();
            }
            return Ok(createdBook);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Book Book)
        {
            _logger.LogInformation("Updating Book with ID {id}", Book.Id);
            var createdBook = _BookServices.Update(Book);
            if (createdBook == null)
            {
                _logger.LogError("Book with ID {id} not found for update", Book.Id);
                return NotFound();
            }
            _logger.LogDebug("Book with ID {id} updated successfully", Book.Id);
            return Ok(createdBook);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Fetching Book with ID {id}", id);
            _BookServices.Delete(id);
            return NoContent();
        }
        
    }
}