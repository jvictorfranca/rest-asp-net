// using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V1;
using RestASPNet.Services;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("api/[controller]/v1")]
    // [EnableCors("LocalPolicy")] // For global CORS
    public class PersonController : ControllerBase
    {
        private readonly IPersonServices _personServices;
        private readonly ILogger<PersonController> _logger;

        public PersonController(IPersonServices personServices, ILogger<PersonController> logger)
        {
            _personServices = personServices;
            _logger = logger;
        }

        [HttpGet(Name ="GetAllPersons")]
        [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        // [EnableCors("LocalPolicy")]
        public IActionResult Get()
        {
            _logger.LogInformation("Fetching all people");
            return Ok(_personServices.FindAll());
        }

        [HttpGet("find-by-name", Name = "GetAllPersonsByName")]
        [ProducesResponseType(200, Type = typeof(List<PersonDTO>))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        // [EnableCors("LocalPolicy")]
        public IActionResult GetByName(
            [FromQuery] string? firstName,
            [FromQuery] string? lastName
            )
        {
            _logger.LogInformation("Fetching all people by name");
            return Ok(_personServices.FindByName(firstName, lastName));
        }

        [HttpGet("{id}", Name ="GetPersonById")]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
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

        [HttpPost(Name = "CreatePerson")]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        // [EnableCors("MultipleOrigin")]
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

        [HttpPut(Name ="UpdatePerson")]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
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

        [HttpDelete("{id}", Name ="DeletePerson")]
        [ProducesResponseType(204, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        [ProducesResponseType(401)]
        public IActionResult Delete(int id)
        {
            _logger.LogInformation("Fetching person with ID {id}", id);
            _personServices.Delete(id);
            return NoContent();
        }

        [HttpPatch("{id}", Name ="DisablePerson")]
        [ProducesResponseType(200, Type = typeof(PersonDTO))]
        [ProducesResponseType(400)]
        public IActionResult Disable(long id)
        {
            _logger.LogInformation("Disabling person with ID {id}", id);
            var disabledPerson = _personServices.Disable(id);
            if (disabledPerson == null)
            {
                _logger.LogWarning("Person with ID {id} not found for disable", id);
                return NotFound();
            }
            _logger.LogDebug("Person with ID {id} disabled successfully", id);
            return Ok(disabledPerson);
        }
    }
}