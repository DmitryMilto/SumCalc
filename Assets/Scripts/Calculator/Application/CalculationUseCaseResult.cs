namespace SumCalc.Calculator.Application
{
    public readonly struct CalculationUseCaseResult
    {
        private CalculationUseCaseResult(bool isSuccess, string output, string historyEntry)
        {
            IsSuccess = isSuccess;
            Output = output;
            HistoryEntry = historyEntry;
        }

        public bool IsSuccess { get; }

        public string Output { get; }

        public string HistoryEntry { get; }

        public static CalculationUseCaseResult Success(string output, string historyEntry)
        {
            return new CalculationUseCaseResult(true, output, historyEntry);
        }

        public static CalculationUseCaseResult Error(string historyEntry)
        {
            return new CalculationUseCaseResult(false, "Error", historyEntry);
        }
    }
}
