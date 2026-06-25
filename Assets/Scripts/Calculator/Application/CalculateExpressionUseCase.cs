using SumCalc.Calculator.Core;

namespace SumCalc.Calculator.Application
{
    public sealed class CalculateExpressionUseCase
    {
        private readonly ExpressionCalculator _calculator;

        public CalculateExpressionUseCase(ExpressionCalculator calculator)
        {
            _calculator = calculator;
        }

        public CalculationUseCaseResult Execute(string expression)
        {
            var result = _calculator.Calculate(expression);
            if (!result.IsSuccess)
            {
                return CalculationUseCaseResult.Error();
            }

            var normalizedExpression = expression ?? string.Empty;
            var output = result.Sum.ToString();
            var historyEntry = $"{normalizedExpression}={output}";
            return CalculationUseCaseResult.Success(output, historyEntry);
        }
    }
}
