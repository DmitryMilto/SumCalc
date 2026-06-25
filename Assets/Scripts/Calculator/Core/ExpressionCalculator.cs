using System.Text.RegularExpressions;

namespace SumCalc.Calculator.Core
{
    public sealed class ExpressionCalculator
    {
        private static readonly Regex ExpressionPattern = new(@"^\d+\+\d+$", RegexOptions.Compiled);

        public CalculationResult Calculate(string expression)
        {
            if (string.IsNullOrWhiteSpace(expression) || !ExpressionPattern.IsMatch(expression))
            {
                return CalculationResult.Error();
            }

            string[] parts = expression.Split('+');
            if (parts.Length != 2)
            {
                return CalculationResult.Error();
            }

            if (!int.TryParse(parts[0], out int left) || !int.TryParse(parts[1], out int right))
            {
                return CalculationResult.Error();
            }

            return CalculationResult.Success(left + right);
        }
    }
}
