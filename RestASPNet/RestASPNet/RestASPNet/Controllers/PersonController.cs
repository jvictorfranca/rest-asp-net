using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RestASPNet.Controllers.Model;
using RestASPNet.Services;

namespace RestASPNet.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class PersonController : ControllerBase
    {
        private readonly IPersonServices _personServices;

        public PersonController(IPersonServices personServices)
        {
            _personServices = personServices;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(_personServices.FindAll());
        }

        [HttpGet("{id}")]
        public IActionResult Get(long id)
        {
            var person = _personServices.FindById(id);
            if (person == null)
            {
                return NotFound();
            }
            return Ok(person);

        }

        [HttpPost]
        public IActionResult Post([FromBody] Person person)
         {
            var createdPerson = _personServices.Create(person);
            if (createdPerson == null)
            {
                return NotFound();
            }
            return Ok(createdPerson);
        }

        [HttpPut]
        public IActionResult Put([FromBody] Person person)
        {
            var createdPerson = _personServices.Update(person);
            if (createdPerson == null)
            {
                return NotFound();
            }
            return Ok(createdPerson);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            _personServices.Delete(id);
            return NoContent();
        }
        
    }
}