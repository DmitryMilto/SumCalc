namespace SumCalc.Calculator.Application
{
    public sealed class SaveCalculatorStateUseCase
    {
        private readonly ICalculatorStateRepository _repository;

        public SaveCalculatorStateUseCase(ICalculatorStateRepository repository)
        {
            _repository = repository;
        }

        public void Execute(CalculatorState state)
        {
            _repository.Save(state);
        }
    }
}
