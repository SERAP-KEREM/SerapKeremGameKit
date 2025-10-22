using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public sealed class UIManagerHUD : UIScreen
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private UIRootController _uiRoot;

        private void Awake()
        {
            if (_restartButton != null) _restartButton.onClick.AddListener(OnRestartClicked);
            if (_settingsButton != null) _settingsButton.onClick.AddListener(OnSettingsClicked);
        }

        private void OnDestroy()
        {
            if (_restartButton != null) _restartButton.onClick.RemoveListener(OnRestartClicked);
            if (_settingsButton != null) _settingsButton.onClick.RemoveListener(OnSettingsClicked);
        }

        public void SetLevelIndex(int levelIndex)
        {
            if (_levelText != null)
                _levelText.text = $"Level {levelIndex + 1}";
        }

        private void OnRestartClicked()
        {
            if (_uiRoot == null) _uiRoot = FindObjectOfType<UIRootController>(true);
            if (_uiRoot != null) _uiRoot.OnRestartRequested();
        }

        private void OnSettingsClicked()
        {
            if (_uiRoot == null) _uiRoot = FindObjectOfType<UIRootController>(true);
            if (_uiRoot != null) _uiRoot.OnOpenSettings();
        }
    }
}



