using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public sealed class RetryScreen : UIScreen
    {
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private UIRootController _uiRoot;

        private void Awake()
        {
            if (_yesButton != null) _yesButton.onClick.AddListener(OnYes);
            if (_noButton != null) _noButton.onClick.AddListener(OnNo);
        }

        private void OnDestroy()
        {
            if (_yesButton != null) _yesButton.onClick.RemoveListener(OnYes);
            if (_noButton != null) _noButton.onClick.RemoveListener(OnNo);
        }

        private void OnYes()
        {
            if (_uiRoot == null) _uiRoot = FindObjectOfType<UIRootController>(true);
            if (_uiRoot != null) _uiRoot.OnRestartConfirmed();
        }

        private void OnNo()
        {
            Hide();
        }
    }
}



