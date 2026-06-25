using System.IO;
using SumCalc.Calculator.Application;
using UnityEngine;

namespace SumCalc.Unity.Infrastructure
{
    public sealed class JsonFileCalculatorStateRepository : ICalculatorStateRepository
    {
        private readonly string _filePath;

        public JsonFileCalculatorStateRepository(string fileName)
        {
            string safeFileName = string.IsNullOrWhiteSpace(fileName) ? "calculator-state.json" : fileName;
            _filePath = Path.Combine(Application.persistentDataPath, safeFileName);
        }

        public CalculatorState Load()
        {
            if (!File.Exists(_filePath))
            {
                return new CalculatorState();
            }

            string json = File.ReadAllText(_filePath);
            if (string.IsNullOrWhiteSpace(json))
            {
                return new CalculatorState();
            }

            CalculatorState state = JsonUtility.FromJson<CalculatorState>(json);
            return state ?? new CalculatorState();
        }

        public void Save(CalculatorState state)
        {
            string directoryPath = Path.GetDirectoryName(_filePath);
            if (!string.IsNullOrWhiteSpace(directoryPath))
            {
                Directory.CreateDirectory(directoryPath);
            }

            string json = JsonUtility.ToJson(state, true);
            File.WriteAllText(_filePath, json);
        }
    }
}
