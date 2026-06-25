using SumCalc.Calculator.Application;
using UnityEngine;

namespace SumCalc.Unity.Infrastructure
{
    public sealed class PlayerPrefsCalculatorStateRepository : ICalculatorStateRepository
    {
        private const string StateKey = "sumcalc.calculator.state";

        public CalculatorState Load()
        {
            if (!PlayerPrefs.HasKey(StateKey))
            {
                return new CalculatorState();
            }

            var json = PlayerPrefs.GetString(StateKey, string.Empty);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new CalculatorState();
            }

            var state = JsonUtility.FromJson<CalculatorState>(json);
            return state ?? new CalculatorState();
        }

        public void Save(CalculatorState state)
        {
            var json = JsonUtility.ToJson(state);
            PlayerPrefs.SetString(StateKey, json);
            PlayerPrefs.Save();
        }
    }
}
