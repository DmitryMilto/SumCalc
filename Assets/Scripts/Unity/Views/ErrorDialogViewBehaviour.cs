using System;
using SumCalc.Dialogs;
using UnityEngine;
using UnityEngine.UI;

namespace SumCalc.Unity.Views
{
    public sealed class ErrorDialogViewBehaviour : MonoBehaviour, IErrorDialogView
    {
        [SerializeField] private GameObject root;
        [SerializeField] private Text messageText;
        [SerializeField] private Button closeButton;

        private CanvasGroup _canvasGroup;

        public event Action Closed;

        private void Awake()
        {
            if (root == null)
            {
                root = gameObject;
            }

            _canvasGroup = root.GetComponent<CanvasGroup>();
            if (_canvasGroup == null)
            {
                _canvasGroup = root.AddComponent<CanvasGroup>();
            }

            closeButton.onClick.AddListener(HandleCloseClicked);
            Hide();
        }

        public void Show(string message)
        {
            if (messageText != null)
            {
                messageText.text = message;
            }

            SetVisible(true);
        }

        public void Hide()
        {
            SetVisible(false);
        }

        private void OnDestroy()
        {
            closeButton?.onClick.RemoveListener(HandleCloseClicked);
            Closed = null;
        }

        private void HandleCloseClicked()
        {
            Hide();
            Closed?.Invoke();
        }

        private void SetVisible(bool isVisible)
        {
            if (_canvasGroup == null)
            {
                return;
            }

            _canvasGroup.alpha = isVisible ? 1f : 0f;
            _canvasGroup.interactable = isVisible;
            _canvasGroup.blocksRaycasts = isVisible;
        }
    }
}
