namespace RestASPNet.Utils
{
    public class NumberHelper
    {
        public static decimal ConvertToDecimal(string numberString)
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

        public static bool IsNumeric(string stringNumber)
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
