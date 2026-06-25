using SumCalc.Unity.Bootstrap;
using SumCalc.Unity.Views;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace SumCalc.Editor
{
    public static class SampleSceneBuilder
    {
        private const string ScenePath = "Assets/Scenes/SampleScene.unity";

        [MenuItem("Tools/SumCalc/Rebuild Sample Scene")]
        public static void RebuildSampleSceneFromMenu()
        {
            BuildSampleScene();
        }

        public static void BuildSampleScene()
        {
            Scene scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);
            scene.name = "SampleScene";

            CreateCamera();
            Canvas canvas = CreateCanvas();
            CreateEventSystem();

            CalculatorViewBehaviour calculatorView = CreateCalculatorScreen(canvas.transform);
            ErrorDialogViewBehaviour dialogView = CreateErrorDialog(canvas.transform);
            CreateBootstrap(calculatorView, dialogView);

            EditorSceneManager.SaveScene(scene, ScenePath);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
        }

        private static void CreateCamera()
        {
            GameObject cameraObject = new("Main Camera");
            cameraObject.tag = "MainCamera";

            Camera camera = cameraObject.AddComponent<Camera>();
            camera.orthographic = true;
            camera.orthographicSize = 5f;
            camera.clearFlags = CameraClearFlags.SolidColor;
            camera.backgroundColor = new Color(0.19215687f, 0.3019608f, 0.4745098f, 0f);

            cameraObject.AddComponent<AudioListener>();
            cameraObject.transform.position = new Vector3(0f, 0f, -10f);
        }

        private static Canvas CreateCanvas()
        {
            GameObject canvasObject = new("Canvas");
            Canvas canvas = canvasObject.AddComponent<Canvas>();
            canvas.renderMode = RenderMode.ScreenSpaceOverlay;

            CanvasScaler scaler = canvasObject.AddComponent<CanvasScaler>();
            scaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;
            scaler.referenceResolution = new Vector2(1080f, 1920f);
            scaler.screenMatchMode = CanvasScaler.ScreenMatchMode.MatchWidthOrHeight;
            scaler.matchWidthOrHeight = 0.5f;

            canvasObject.AddComponent<GraphicRaycaster>();
            return canvas;
        }

        private static void CreateEventSystem()
        {
            GameObject eventSystemObject = new("EventSystem");
            eventSystemObject.AddComponent<EventSystem>();
            eventSystemObject.AddComponent<InputSystemUIInputModule>();
        }

        private static CalculatorViewBehaviour CreateCalculatorScreen(Transform parent)
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject screenRoot = CreateUiObject("CalculatorScreen", parent);
            Stretch(screenRoot.GetComponent<RectTransform>(), 0f, 0f, 0f, 0f);

            GameObject panel = CreatePanel("MainPanel", screenRoot.transform, new Color(0.94f, 0.95f, 0.98f));
            Stretch(panel.GetComponent<RectTransform>(), 40f, 40f, -40f, -40f);

            CreateLabel("Title", panel.transform, font, "Калькулятор", 34, FontStyle.Bold, TextAnchor.MiddleCenter, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -40f), new Vector2(500f, 50f));
            CreateLabel("ExpressionLabel", panel.transform, font, "Выражение", 20, FontStyle.Bold, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(20f, -100f), new Vector2(220f, 28f));

            InputField inputField = CreateInputField(panel.transform, font, new Vector2(20f, -145f), new Vector2(-20f, -95f));
            Button calculateButton = CreateButton(panel.transform, font, "CalculateButton", "Вычислить", new Vector2(20f, -205f), new Vector2(240f, -155f), new Color(0.17f, 0.45f, 0.9f));

            CreateLabel("ResultCaption", panel.transform, font, "Результат", 20, FontStyle.Bold, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(20f, -260f), new Vector2(180f, 28f));
            Text resultText = CreateValueText(panel.transform, font, "ResultValue", new Vector2(20f, -315f), new Vector2(-20f, -265f));

            CreateLabel("HistoryCaption", panel.transform, font, "История", 20, FontStyle.Bold, TextAnchor.MiddleLeft, new Vector2(0f, 1f), new Vector2(0f, 1f), new Vector2(20f, -375f), new Vector2(150f, 28f));
            Text historyText = CreateHistoryText(panel.transform, font, new Vector2(20f, 20f), new Vector2(-20f, -415f));

            var view = screenRoot.AddComponent<CalculatorViewBehaviour>();
            SetSerializedField(view, "inputField", inputField);
            SetSerializedField(view, "calculateButton", calculateButton);
            SetSerializedField(view, "resultText", resultText);
            SetSerializedField(view, "historyText", historyText);
            return view;
        }

        private static ErrorDialogViewBehaviour CreateErrorDialog(Transform parent)
        {
            Font font = Resources.GetBuiltinResource<Font>("LegacyRuntime.ttf");

            GameObject overlay = CreateUiObject("ErrorDialogOverlay", parent);
            Image overlayImage = overlay.AddComponent<Image>();
            overlayImage.color = new Color(0f, 0f, 0f, 0.45f);
            Stretch(overlay.GetComponent<RectTransform>(), 0f, 0f, 0f, 0f);

            GameObject dialogPanel = CreatePanel("ErrorDialogPanel", overlay.transform, new Color(1f, 0.98f, 0.98f));
            RectTransform dialogRect = dialogPanel.GetComponent<RectTransform>();
            dialogRect.anchorMin = new Vector2(0.5f, 0.5f);
            dialogRect.anchorMax = new Vector2(0.5f, 0.5f);
            dialogRect.pivot = new Vector2(0.5f, 0.5f);
            dialogRect.sizeDelta = new Vector2(540f, 240f);
            dialogRect.anchoredPosition = Vector2.zero;

            CreateLabel("ErrorTitle", dialogPanel.transform, font, "Ошибка", 28, FontStyle.Bold, TextAnchor.MiddleCenter, new Vector2(0.5f, 1f), new Vector2(0.5f, 1f), new Vector2(0f, -40f), new Vector2(220f, 40f));
            Text messageText = CreateLabel("ErrorMessage", dialogPanel.transform, font, string.Empty, 20, FontStyle.Normal, TextAnchor.MiddleCenter, new Vector2(0f, 0.5f), new Vector2(1f, 0.5f), new Vector2(0f, 0f), new Vector2(-60f, 90f));
            Button closeButton = CreateButton(dialogPanel.transform, font, "CloseButton", "Закрыть", new Vector2(170f, 20f), new Vector2(370f, 70f), new Color(0.78f, 0.21f, 0.21f));

            var view = overlay.AddComponent<ErrorDialogViewBehaviour>();
            SetSerializedField(view, "root", overlay);
            SetSerializedField(view, "messageText", messageText);
            SetSerializedField(view, "closeButton", closeButton);
            overlay.SetActive(false);
            return view;
        }

        private static void CreateBootstrap(CalculatorViewBehaviour calculatorView, ErrorDialogViewBehaviour dialogView)
        {
            GameObject bootstrapObject = new("AppBootstrap");
            CalculatorAppBootstrap bootstrap = bootstrapObject.AddComponent<CalculatorAppBootstrap>();
            SetSerializedField(bootstrap, "calculatorView", calculatorView);
            SetSerializedField(bootstrap, "errorDialogView", dialogView);
        }

        private static GameObject CreatePanel(string name, Transform parent, Color color)
        {
            GameObject panel = CreateUiObject(name, parent);
            Image image = panel.AddComponent<Image>();
            image.color = color;
            return panel;
        }

        private static InputField CreateInputField(Transform parent, Font font, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject fieldObject = CreateUiObject("ExpressionInput", parent);
            Image background = fieldObject.AddComponent<Image>();
            background.color = Color.white;

            InputField inputField = fieldObject.AddComponent<InputField>();
            RectTransform rect = fieldObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Text placeholder = CreateLabel("Placeholder", fieldObject.transform, font, "54+21", 22, FontStyle.Italic, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(16f, 0f), new Vector2(-16f, 0f));
            placeholder.color = new Color(0.55f, 0.58f, 0.62f);

            Text text = CreateLabel("Text", fieldObject.transform, font, string.Empty, 22, FontStyle.Normal, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(16f, 0f), new Vector2(-16f, 0f));
            text.color = new Color(0.1f, 0.12f, 0.18f);

            inputField.placeholder = placeholder;
            inputField.textComponent = text;
            inputField.lineType = InputField.LineType.SingleLine;
            inputField.contentType = InputField.ContentType.Standard;
            return inputField;
        }

        private static Button CreateButton(Transform parent, Font font, string name, string label, Vector2 offsetMin, Vector2 offsetMax, Color color)
        {
            GameObject buttonObject = CreateUiObject(name, parent);
            Image image = buttonObject.AddComponent<Image>();
            image.color = color;

            Button button = buttonObject.AddComponent<Button>();
            ColorBlock colors = button.colors;
            colors.highlightedColor = color * 1.05f;
            colors.pressedColor = color * 0.9f;
            button.colors = colors;

            RectTransform rect = buttonObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            CreateLabel("Label", buttonObject.transform, font, label, 20, FontStyle.Bold, TextAnchor.MiddleCenter, new Vector2(0f, 0f), new Vector2(1f, 1f), Vector2.zero, Vector2.zero);
            return button;
        }

        private static Text CreateValueText(Transform parent, Font font, string name, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject valueObject = CreateUiObject(name, parent);
            Image background = valueObject.AddComponent<Image>();
            background.color = Color.white;

            Text text = CreateLabel("Text", valueObject.transform, font, string.Empty, 22, FontStyle.Bold, TextAnchor.MiddleLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(16f, 0f), new Vector2(-16f, 0f));
            text.color = new Color(0.1f, 0.12f, 0.18f);

            RectTransform rect = valueObject.GetComponent<RectTransform>();
            rect.anchorMin = new Vector2(0f, 1f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;
            return text;
        }

        private static Text CreateHistoryText(Transform parent, Font font, Vector2 offsetMin, Vector2 offsetMax)
        {
            GameObject historyObject = CreateUiObject("HistoryPanel", parent);
            Image background = historyObject.AddComponent<Image>();
            background.color = Color.white;

            RectTransform rect = historyObject.GetComponent<RectTransform>();
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = new Vector2(1f, 1f);
            rect.offsetMin = offsetMin;
            rect.offsetMax = offsetMax;

            Text text = CreateLabel("Text", historyObject.transform, font, "История пуста", 20, FontStyle.Normal, TextAnchor.UpperLeft, new Vector2(0f, 0f), new Vector2(1f, 1f), new Vector2(16f, 16f), new Vector2(-16f, -16f));
            text.color = new Color(0.14f, 0.16f, 0.22f);
            text.verticalOverflow = VerticalWrapMode.Overflow;
            text.horizontalOverflow = HorizontalWrapMode.Wrap;
            return text;
        }

        private static Text CreateLabel(string name, Transform parent, Font font, string value, int fontSize, FontStyle fontStyle, TextAnchor alignment, Vector2 anchorMin, Vector2 anchorMax, Vector2 anchoredPosition, Vector2 sizeDelta)
        {
            GameObject labelObject = CreateUiObject(name, parent);
            Text text = labelObject.AddComponent<Text>();
            text.font = font;
            text.text = value;
            text.fontSize = fontSize;
            text.fontStyle = fontStyle;
            text.alignment = alignment;
            text.color = new Color(0.1f, 0.12f, 0.18f);

            RectTransform rect = labelObject.GetComponent<RectTransform>();
            rect.anchorMin = anchorMin;
            rect.anchorMax = anchorMax;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.anchoredPosition = anchoredPosition;
            rect.sizeDelta = sizeDelta;
            return text;
        }

        private static GameObject CreateUiObject(string name, Transform parent)
        {
            GameObject gameObject = new(name, typeof(RectTransform));
            gameObject.transform.SetParent(parent, false);
            return gameObject;
        }

        private static void Stretch(RectTransform rect, float left, float bottom, float right, float top)
        {
            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.offsetMin = new Vector2(left, bottom);
            rect.offsetMax = new Vector2(right, top);
        }

        private static void SetSerializedField(Object target, string fieldName, Object value)
        {
            SerializedObject serializedObject = new(target);
            serializedObject.FindProperty(fieldName).objectReferenceValue = value;
            serializedObject.ApplyModifiedPropertiesWithoutUndo();
        }
    }
}
