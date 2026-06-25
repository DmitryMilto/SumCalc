using System;
using System.Collections.Generic;

namespace SumCalc.Calculator.Presentation
{
    public interface ICalculatorView
    {
        event Action<string> InputChanged;

        event Action CalculateRequested;

        void SetExpression(string expression);

        void SetResult(string result);

        void SetHistory(IReadOnlyList<string> history);
    }
}
