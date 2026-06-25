using System;
using System.Collections.Generic;

namespace SumCalc.Calculator.Application
{
    [Serializable]
    public sealed class CalculatorState
    {
        public CalculatorState()
        {
            CurrentExpression = string.Empty;
            History = new List<string>();
        }

        public CalculatorState(string currentExpression, IReadOnlyList<string> history)
        {
            CurrentExpression = currentExpression ?? string.Empty;
            History = new List<string>(history ?? Array.Empty<string>());
        }

        public string CurrentExpression;

        public List<string> History;
    }
}
