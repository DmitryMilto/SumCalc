using System;
using System.Collections.Generic;
using System.Linq;
using SumCalc.Calculator.Presentation;
using UnityEngine;
using UnityEngine.UI;

namespace SumCalc.Unity.Views
{
    public sealed class CalculatorViewBehaviour : MonoBehaviour, ICalculatorView
    {
        [SerializeField] private InputField inputField;
        [SerializeField] private Button calculateButton;
        [SerializeField] private Text resultText;
        [SerializeField] private Text historyText;
        [SerializeField] private ScrollRect historyScrollRect;

        public event Action<string> InputChanged;

        public event Action CalculateRequested;

        private void Awake()
        {
            inputField.onValueChanged.AddListener(HandleInputChanged);
            calculateButton.onClick.AddListener(HandleCalculateClicked);
        }

        public void SetExpression(string expression)
        {
            inputField?.SetTextWithoutNotify(expression ?? string.Empty);
        }

        public void SetResult(string result)
        {
            if (resultText != null)
            {
                resultText.text = result ?? string.Empty;
            }
        }

        public void SetHistory(IReadOnlyList<string> history)
        {
            if (historyText == null)
            {
                return;
            }

            if (history == null || history.Count <= 1)
            {
                historyText.text = string.Empty;
                return;
            }

            historyText.text = string.Join(Environment.NewLine, history.Skip(1));
            if (historyScrollRect != null)
            {
                Canvas.ForceUpdateCanvases();
                historyScrollRect.verticalNormalizedPosition = 1f;
            }
        }

        private void OnDestroy()
        {
            inputField?.onValueChanged.RemoveListener(HandleInputChanged);
            calculateButton?.onClick.RemoveListener(HandleCalculateClicked);
            InputChanged = null;
            CalculateRequested = null;
        }

        private void HandleInputChanged(string expression)
        {
            InputChanged?.Invoke(expression);
        }

        private void HandleCalculateClicked()
        {
            CalculateRequested?.Invoke();
        }
    }
}
