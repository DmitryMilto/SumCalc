namespace SumCalc.Calculator.Application
{
    public sealed class LoadCalculatorStateUseCase
    {
        private readonly ICalculatorStateRepository _repository;

        public LoadCalculatorStateUseCase(ICalculatorStateRepository repository)
        {
            _repository = repository;
        }

        public CalculatorState Execute()
        {
            return _repository.Load();
        }
    }
}
