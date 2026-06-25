using SumCalc.Calculator.Application;
using SumCalc.Calculator.Core;
using SumCalc.Calculator.Presentation;
using SumCalc.Dialogs;
using SumCalc.Unity.Infrastructure;
using SumCalc.Unity.Views;
using UnityEngine;

namespace SumCalc.Unity.Bootstrap
{
    public sealed class CalculatorAppBootstrap : MonoBehaviour
    {
        [SerializeField] private CalculatorViewBehaviour calculatorView;
        [SerializeField] private ErrorDialogViewBehaviour errorDialogView;
        [SerializeField] private CalculatorStateStorageMode storageMode = CalculatorStateStorageMode.PlayerPrefs;
        [SerializeField] private string jsonFileName = "calculator-state.json";

        private CalculatorPresenter _calculatorPresenter;
        private ErrorDialogPresenter _errorDialogPresenter;

        private void Awake()
        {
            if (calculatorView == null || errorDialogView == null)
            {
                Debug.LogError("CalculatorAppBootstrap requires scene references to calculator and dialog views.");
                return;
            }

            var repository = CreateRepository();
            var calculator = new ExpressionCalculator();

            _errorDialogPresenter = new ErrorDialogPresenter(errorDialogView);
            _calculatorPresenter = new CalculatorPresenter(
                calculatorView,
                new CalculateExpressionUseCase(calculator),
                new LoadCalculatorStateUseCase(repository),
                new SaveCalculatorStateUseCase(repository),
                _errorDialogPresenter);

            _calculatorPresenter.Initialize();
        }

        private void OnDestroy()
        {
            _calculatorPresenter?.Dispose();
            _errorDialogPresenter?.Dispose();
        }

        private ICalculatorStateRepository CreateRepository()
        {
            var playerPrefsRepository = new PlayerPrefsCalculatorStateRepository();
            var jsonFileRepository = new JsonFileCalculatorStateRepository(jsonFileName);

            return storageMode switch
            {
                CalculatorStateStorageMode.JsonFile => jsonFileRepository,
                CalculatorStateStorageMode.PlayerPrefsAndJsonFile => new CompositeCalculatorStateRepository(playerPrefsRepository, jsonFileRepository),
                _ => playerPrefsRepository
            };
        }
    }
}
