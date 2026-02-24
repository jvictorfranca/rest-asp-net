using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Data.DTO.V2;
using RestASPNet.Services;
using RestASPNet.Services.Impl;

namespace RestASPNet.Controllers.V2
{
    [ApiController]
    [Route("api/[controller]/v2")]
    public class PersonController : ControllerBase
    {
        private readonly PersonServicesImplV2 _personServices;
        private readonly ILogger<PersonController> _logger;

        public PersonController(PersonServicesImplV2 personServices, ILogger<PersonController> logger)
        {
            _personServices = personServices;
            _logger = logger;
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
            return Ok(createdPerson);
        }
        
    }
}