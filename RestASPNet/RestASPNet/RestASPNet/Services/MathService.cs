namespace RestASPNet.Services
{
    public class MathService
    {
        public decimal sum(decimal firstNumber, decimal secondNumber)
        {
            return firstNumber + secondNumber;
        }

        public decimal sub(decimal firstNumber, decimal secondNumber)
        {
            return firstNumber - secondNumber;
        }

        public decimal div(decimal firstNumber, decimal secondNumber)
        {
            return firstNumber / secondNumber;
        }

        public decimal mult(decimal firstNumber, decimal secondNumber)
        {
            return firstNumber * secondNumber;
        }

        public decimal avg(decimal firstNumber, decimal secondNumber)
        {
            return (firstNumber + secondNumber)/2;
        }

        public decimal root(decimal firstNumber)
        {
            return (decimal) Math.Sqrt((double) firstNumber);
        }

    }
}
