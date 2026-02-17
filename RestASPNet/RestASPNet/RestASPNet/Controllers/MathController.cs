using Microsoft.AspNetCore.Mvc;
using RestASPNet.Services;
using RestASPNet.Utils;
using System.Runtime.CompilerServices;

namespace RestASPNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MathController : ControllerBase
    {
        private readonly MathService _service;

        public MathController(MathService service)
        {
            _service = service;
        }

        [HttpGet("sum/{firstNumber}/{secondNumber}")]
        public IActionResult GetSum(string firstNumber, string secondNumber)
        {

            if(NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber))
            {

            var sum = _service.sum(NumberHelper.ConvertToDecimal(firstNumber) , NumberHelper.ConvertToDecimal(secondNumber));
            return Ok(sum);

            } else
            {
                return BadRequest("Invalid Input");
            }
        }

        [HttpGet("sub/{firstNumber}/{secondNumber}")]
        public IActionResult GetSub(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber))
            {

                var sub = _service.sub(NumberHelper.ConvertToDecimal(firstNumber), NumberHelper.ConvertToDecimal(secondNumber));
                return Ok(sub);

            }
            else
            {
                return BadRequest("Invalid Input");
            }
        }

        [HttpGet("div/{firstNumber}/{secondNumber}")]
        public IActionResult GetDiv(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber))
            {

                var div = _service.div(NumberHelper.ConvertToDecimal(firstNumber),NumberHelper.ConvertToDecimal(secondNumber));
                return Ok(div);

            }
            else
            {
                return BadRequest("Invalid Input");
            }
        }

        [HttpGet("mult/{firstNumber}/{secondNumber}")]
        public IActionResult GetMult(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber))
            {

                var mult = _service.mult(NumberHelper.ConvertToDecimal(firstNumber), NumberHelper.ConvertToDecimal(secondNumber));
                return Ok(mult);

            }
            else
            {
                return BadRequest("Invalid Input");
            }
        }


        [HttpGet("root/{firstNumber}")]
        public IActionResult GetRoot(string firstNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber))
            {

                var root = _service.root(NumberHelper.ConvertToDecimal(firstNumber));
                return Ok(root);

            }
            else
            {
                return BadRequest("Invalid Input");
            }
        }

        [HttpGet("avg/{firstNumber}/{secondNumber}")]
        public IActionResult GetAvg(string firstNumber, string secondNumber)
        {
            if (NumberHelper.IsNumeric(firstNumber) && NumberHelper.IsNumeric(secondNumber))
            {

                var avg = _service.avg(NumberHelper.ConvertToDecimal(firstNumber), NumberHelper.ConvertToDecimal(secondNumber));
                return Ok(avg);

            }
            else
            {
                return BadRequest("Invalid Input");
            }
        }
    }
}
