using System.Collections.Generic;
using SumCalc.Calculator.Application;

namespace SumCalc.Unity.Infrastructure
{
    public sealed class CompositeCalculatorStateRepository : ICalculatorStateRepository
    {
        private readonly ICalculatorStateRepository[] _repositories;

        public CompositeCalculatorStateRepository(params ICalculatorStateRepository[] repositories)
        {
            _repositories = repositories ?? new ICalculatorStateRepository[0];
        }

        public CalculatorState Load()
        {
            foreach (ICalculatorStateRepository repository in _repositories)
            {
                if (repository == null)
                {
                    continue;
                }

                CalculatorState state = repository.Load();
                if (HasData(state))
                {
                    return state;
                }
            }

            return new CalculatorState();
        }

        public void Save(CalculatorState state)
        {
            foreach (ICalculatorStateRepository repository in _repositories)
            {
                repository?.Save(state);
            }
        }

        private static bool HasData(CalculatorState state)
        {
            return state != null
                && (!string.IsNullOrWhiteSpace(state.CurrentExpression)
                    || (state.History != null && state.History.Count > 0));
        }
    }
}
