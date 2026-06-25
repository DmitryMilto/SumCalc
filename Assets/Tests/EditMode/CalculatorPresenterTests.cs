using System;
using System.Collections.Generic;
using NUnit.Framework;
using SumCalc.Calculator.Application;
using SumCalc.Calculator.Core;
using SumCalc.Calculator.Presentation;
using SumCalc.Dialogs;

namespace SumCalc.Tests.EditMode
{
    public sealed class CalculatorPresenterTests
    {
        [Test]
        public void CalculateExpressionUseCase_ReturnsSum_ForValidExpression()
        {
            var useCase = new CalculateExpressionUseCase(new ExpressionCalculator());

            CalculationUseCaseResult result = useCase.Execute("54+21");

            Assert.That(result.IsSuccess, Is.True);
            Assert.That(result.Output, Is.EqualTo("75"));
            Assert.That(result.HistoryEntry, Is.EqualTo("54+21=75"));
        }

        [Test]
        public void CalculateExpressionUseCase_ReturnsError_ForInvalidExpression()
        {
            var useCase = new CalculateExpressionUseCase(new ExpressionCalculator());

            CalculationUseCaseResult result = useCase.Execute("98.12+48.1");

            Assert.That(result.IsSuccess, Is.False);
            Assert.That(result.Output, Is.EqualTo("Error"));
        }

        [Test]
        public void Presenter_ShowsError_AndRestoresExpression_WhenCalculationFails()
        {
            var view = new FakeCalculatorView();
            var repository = new InMemoryStateRepository();
            var dialogView = new FakeErrorDialogView();
            using var dialogPresenter = new ErrorDialogPresenter(dialogView);
            using var presenter = new CalculatorPresenter(
                view,
                new CalculateExpressionUseCase(new ExpressionCalculator()),
                new LoadCalculatorStateUseCase(repository),
                new SaveCalculatorStateUseCase(repository),
                dialogPresenter);

            presenter.Initialize();
            view.RaiseInputChanged("45+-88");
            view.RaiseCalculateRequested();

            Assert.That(view.Result, Is.EqualTo("Error"));
            Assert.That(dialogView.LastMessage, Is.EqualTo("Проверьте введенную информацию."));

            dialogView.RaiseClosed();

            Assert.That(view.Expression, Is.EqualTo("45+-88"));
            Assert.That(repository.State.CurrentExpression, Is.EqualTo("45+-88"));
        }

        [Test]
        public void Presenter_LoadsSavedState_OnInitialize()
        {
            var view = new FakeCalculatorView();
            var repository = new InMemoryStateRepository
            {
                State = new CalculatorState("34+47", new[] { "11+22=33", "6+7=13" })
            };
            var dialogView = new FakeErrorDialogView();
            using var dialogPresenter = new ErrorDialogPresenter(dialogView);
            using var presenter = new CalculatorPresenter(
                view,
                new CalculateExpressionUseCase(new ExpressionCalculator()),
                new LoadCalculatorStateUseCase(repository),
                new SaveCalculatorStateUseCase(repository),
                dialogPresenter);

            presenter.Initialize();

            Assert.That(view.Expression, Is.EqualTo("34+47"));
            Assert.That(view.History, Is.EquivalentTo(new[] { "11+22=33", "6+7=13" }));
        }

        private sealed class FakeCalculatorView : ICalculatorView
        {
            public event Action<string> InputChanged;
            public event Action CalculateRequested;

            public string Expression { get; private set; } = string.Empty;

            public string Result { get; private set; } = string.Empty;

            public IReadOnlyList<string> History { get; private set; } = Array.Empty<string>();

            public void SetExpression(string expression)
            {
                Expression = expression ?? string.Empty;
            }

            public void SetResult(string result)
            {
                Result = result ?? string.Empty;
            }

            public void SetHistory(IReadOnlyList<string> history)
            {
                History = history ?? Array.Empty<string>();
            }

            public void RaiseInputChanged(string expression)
            {
                Expression = expression;
                InputChanged?.Invoke(expression);
            }

            public void RaiseCalculateRequested()
            {
                CalculateRequested?.Invoke();
            }
        }

        private sealed class FakeErrorDialogView : IErrorDialogView
        {
            public event Action Closed;

            public string LastMessage { get; private set; } = string.Empty;

            public void Show(string message)
            {
                LastMessage = message ?? string.Empty;
            }

            public void Hide()
            {
            }

            public void RaiseClosed()
            {
                Closed?.Invoke();
            }
        }

        private sealed class InMemoryStateRepository : ICalculatorStateRepository
        {
            public CalculatorState State { get; set; } = new();

            public CalculatorState Load()
            {
                return new CalculatorState(State.CurrentExpression, State.History);
            }

            public void Save(CalculatorState state)
            {
                State = new CalculatorState(state.CurrentExpression, state.History);
            }
        }
    }
}
