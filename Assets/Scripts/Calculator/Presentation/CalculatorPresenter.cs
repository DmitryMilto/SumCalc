using System;
using System.Collections.Generic;
using SumCalc.Calculator.Application;
using SumCalc.Dialogs;

namespace SumCalc.Calculator.Presentation
{
    public sealed class CalculatorPresenter : IDisposable
    {
        private readonly ICalculatorView _view;
        private readonly CalculateExpressionUseCase _calculateExpressionUseCase;
        private readonly LoadCalculatorStateUseCase _loadStateUseCase;
        private readonly SaveCalculatorStateUseCase _saveStateUseCase;
        private readonly ErrorDialogPresenter _errorDialogPresenter;
        private readonly List<string> _history = new();

        private string _currentExpression = string.Empty;

        public CalculatorPresenter(
            ICalculatorView view,
            CalculateExpressionUseCase calculateExpressionUseCase,
            LoadCalculatorStateUseCase loadStateUseCase,
            SaveCalculatorStateUseCase saveStateUseCase,
            ErrorDialogPresenter errorDialogPresenter)
        {
            _view = view;
            _calculateExpressionUseCase = calculateExpressionUseCase;
            _loadStateUseCase = loadStateUseCase;
            _saveStateUseCase = saveStateUseCase;
            _errorDialogPresenter = errorDialogPresenter;
        }

        public void Initialize()
        {
            CalculatorState state = _loadStateUseCase.Execute();
            _currentExpression = state.CurrentExpression ?? string.Empty;
            _history.Clear();
            if (state.History != null)
            {
                _history.AddRange(state.History);
            }

            _view.InputChanged += OnInputChanged;
            _view.CalculateRequested += OnCalculateRequested;
            _errorDialogPresenter.Closed += OnDialogClosed;

            _view.SetExpression(_currentExpression);
            _view.SetResult(_history.Count > 0 ? _history[0] : string.Empty);
            _view.SetHistory(_history);
        }

        public void Dispose()
        {
            _view.InputChanged -= OnInputChanged;
            _view.CalculateRequested -= OnCalculateRequested;
            _errorDialogPresenter.Closed -= OnDialogClosed;
        }

        private void OnInputChanged(string expression)
        {
            _currentExpression = expression ?? string.Empty;
            SaveState();
        }

        private void OnCalculateRequested()
        {
            var result = _calculateExpressionUseCase.Execute(_currentExpression);
            _history.Insert(0, result.HistoryEntry);
            _view.SetResult(result.HistoryEntry);
            _view.SetHistory(_history);

            if (!result.IsSuccess)
            {
                SaveState();
                _errorDialogPresenter.ShowValidationMessage();
                return;
            }

            SaveState();
        }

        private void OnDialogClosed()
        {
            _view.SetExpression(_currentExpression);
        }

        private void SaveState()
        {
            var state = new CalculatorState(_currentExpression, _history);
            _saveStateUseCase.Execute(state);
        }
    }
}
