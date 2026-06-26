# SumCalc

Unity-проект тестового задания "Калькулятор", в котором поддерживается только операция сложения.

## Что реализовано

- Ввод выражений вида `A+B`, где `A` и `B` - целые неотрицательные числа.
- Вычисление суммы по кнопке `RESULT`.
- Обработка некорректного ввода с выводом `Error`.
- Показ диалога ошибки с просьбой проверить введённое выражение.
- Восстановление выражения в поле ввода после закрытия диалога ошибки.
- Сохранение состояния между сессиями:
  - текущее выражение;
  - история вычислений.
- Отображение истории вычислений, включая ошибочные выражения в формате `expr=ERROR`.
- EditMode-тесты для базовых сценариев calculation/presenter.

## Используемые подходы

- `Clean Architecture`
- `MVP (Model-View-Presenter)`
- Разделение на assembly-модули

Структура модулей:

- `Assets/Scripts/Calculator/Core` - вычисление выражения.
- `Assets/Scripts/Calculator/Application` - use case'ы и состояние приложения.
- `Assets/Scripts/Calculator/Presentation` - presenter и контракт view.
- `Assets/Scripts/Dialogs` - отдельный модуль диалога ошибки.
- `Assets/Scripts/Unity` - Unity-адаптеры, bootstrap, scene bindings и инфраструктура хранения.

## Основные файлы

- [SampleScene.unity](Assets/Scenes/SampleScene.unity)
- [CalculatorAppBootstrap.cs](Assets/Scripts/Unity/Bootstrap/CalculatorAppBootstrap.cs)
- [ExpressionCalculator.cs](Assets/Scripts/Calculator/Core/ExpressionCalculator.cs)
- [CalculatorPresenter.cs](Assets/Scripts/Calculator/Presentation/CalculatorPresenter.cs)
- [ErrorDialogPresenter.cs](Assets/Scripts/Dialogs/ErrorDialogPresenter.cs)

## Правила валидации

Корректным считается выражение формата:

- `54+21`
- `45+00`

Некорректными считаются, например:

- `45+-88`
- `98.12+48.1`
- `12`
- `1+2+3`
- пустая строка

Для разбора используется регулярное выражение `^\d+\+\d+$`.

## Сохранение состояния

По умолчанию используется `PlayerPrefs`.

Поддерживаются три режима хранения:

- `PlayerPrefs`
- `JsonFile`
- `PlayerPrefsAndJsonFile`

Переключение выполняется в [CalculatorAppBootstrap.cs](Assets/Scripts/Unity/Bootstrap/CalculatorAppBootstrap.cs) через поле `storageMode`.

## Запуск

1. Открыть проект в Unity `6000.3.9f1`.
2. Открыть сцену [SampleScene.unity](Assets/Scenes/SampleScene.unity).
3. Нажать `Play`.

## Тесты

EditMode-тесты находятся в:

- [CalculatorPresenterTests.cs](Assets/Tests/EditMode/CalculatorPresenterTests.cs)

Проверяемые сценарии:

- корректное сложение;
- ошибка при некорректном выражении;
- показ диалога и восстановление ввода;
- восстановление сохранённого состояния.

## Примечания

- Сцена поддерживается как обычная Unity-сцена без генератора UI из editor-скрипта.
- История и текущий результат отображаются в формате, близком к макетам из ТЗ.
- Ошибочные операции сохраняются в историю, как показано в приложении A.
