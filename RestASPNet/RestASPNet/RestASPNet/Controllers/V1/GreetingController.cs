using Microsoft.AspNetCore.Mvc;
using RestASPNet.Controllers.Model;
using System.Net.Security;
using System.Runtime.CompilerServices;

namespace RestASPNet.Controllers.V1
{
    [ApiController]
    [Route("[controller]")]
    public class GreetingController : ControllerBase
    {

        private static long _counter = 0;
        private static readonly string _template = "Hello, {0}!"; 
        [HttpGet]
        [Produces("application/json")]
        public Greeting Get([FromQuery] string name = "World")
        {
            var id = Interlocked.Increment(ref _counter);
            var content = string.Format(_template, name);
            return new Greeting(id, content);
        }
    }
}
