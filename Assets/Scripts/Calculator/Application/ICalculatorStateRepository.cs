namespace SumCalc.Calculator.Application
{
    public interface ICalculatorStateRepository
    {
        CalculatorState Load();

        void Save(CalculatorState state);
    }
}
