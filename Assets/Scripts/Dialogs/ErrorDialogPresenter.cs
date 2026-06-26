using System;

namespace SumCalc.Dialogs
{
    public sealed class ErrorDialogPresenter : IDisposable
    {
        private readonly IErrorDialogView _view;

        public ErrorDialogPresenter(IErrorDialogView view)
        {
            _view = view;
            _view.Closed += OnClosed;
        }

        public event Action Closed;

        public void ShowValidationMessage()
        {
            _view.Show("Please check the expression\nyou just entered");
        }

        public void Hide()
        {
            _view.Hide();
        }

        public void Dispose()
        {
            _view.Closed -= OnClosed;
            Closed = null;
        }

        private void OnClosed()
        {
            Closed?.Invoke();
        }
    }
}
