using System;

namespace SumCalc.Dialogs
{
    public interface IErrorDialogView
    {
        event Action Closed;

        void Show(string message);

        void Hide();
    }
}
