namespace SumCalc.Calculator.Core
{
    public readonly struct CalculationResult
    {
        private CalculationResult(bool isSuccess, int sum)
        {
            IsSuccess = isSuccess;
            Sum = sum;
        }

        public bool IsSuccess { get; }

        public int Sum { get; }

        public static CalculationResult Success(int sum)
        {
            return new CalculationResult(true, sum);
        }

        public static CalculationResult Error()
        {
            return new CalculationResult(false, default);
        }
    }
}
