using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Services;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonServices _personServices;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonServices personServices, ILogger<PersonController> logger)
        {
            _personServices = personServices;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all people");
            return Ok(_personServices.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            _logger.LogInformation("Fetching person with ID {id}", id);
            var person = _personServices.FindById(id);
            if (person == null)
            {
                _logger.LogWarning("Person with ID {id} not found", id);
                return NotFound();
            }
            return Ok(person);

        }

        [HttpPost]
        public IActionResult Post([FromBody] PersonDTO person)
         {
            _logger.LogInformation("Creating new person {firstName}", person.FirstName);
            var createdPerson = _personServices.Create(person);
            if (createdPerson == null)
            {
                _logger.LogError("Failed to create person {firstName}", person.FirstName);
                return NotFound();
            }
            Response.Headers.Add("X-API-Deprecated", "true");
            Response.Headers.Add("X-API-Deprecation-Date", "2026-12-31");
            return Ok(createdPerson);
        }

        [HttpPut]
        public IActionResult Put([FromBody] PersonDTO person)
        {
            _logger.LogInformation("Updating person with ID {id}", person.Id);
            var createdPerson = _personServices.Update(person);
            if (createdPerson == null)
            {
                _logger.LogError("Person with ID {id} not found for update", person.Id);
                return NotFound();
            }
            _logger.LogDebug("Person with ID {id} updated successfully", person.Id);
            return Ok(createdPerson);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Fetching person with ID {id}", id);
            _personServices.Delete(id);
            return NoContent();
        }
        
    }
}