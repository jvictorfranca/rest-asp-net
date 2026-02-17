using Microsoft.AspNetCore.Mvc;

namespace RestASPNet.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MathController : ControllerBase
    {
        [HttpGet("sum/{firstNumber}/{secondNumber}")]
        public IActionResult Get(string firstNumber, string secondNumber)
        {
            if(IsNumeric(firstNumber) && IsNumeric(secondNumber))
            {

            var sum = ConvertToDecimal(firstNumber) + ConvertToDecimal(secondNumber);
            return Ok(sum);

            } else
            {
                return BadRequest("Invalid Input");
            }
        }

        private decimal ConvertToDecimal(string numberString)
        {
            decimal decimalValue;
            if (decimal.TryParse(
                numberString,
                System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.InvariantInfo,
                out decimalValue))
            {
                return decimalValue;
            }
            else 
            {
                return 0; 
            }
        }

        private bool IsNumeric(string stringNumber)
        {
            decimal decimalNumber = 0;
            bool isNumber = decimal.TryParse(
                stringNumber,
                System.Globalization.NumberStyles.Any,
                System.Globalization.NumberFormatInfo.InvariantInfo,
                out decimalNumber);
            return isNumber;
        }
    }
}
