using SerapKeremGameKit._Managers;
using SerapKeremGameKit._Singletons;
using TMPro;
using TriInspector;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public class UIManager : MonoSingleton<UIManager>
    {
        [SerializeField] private TextMeshProUGUI _levelText;

        [Title("Panels")]
        [SerializeField] private WinScreen _winPanel;
        [SerializeField] private FailScreen _failScreen;
        [SerializeField] private SettingsScreen _settingsScreen;
        // [SerializeField] private LevelStartScreen _levelStartScreen;
        public InGameUI InGameUI { get; private set; }

        [SerializeField] private Button _settingsButton;

        protected override void Awake()
        {
            base.Awake();

            InGameUI = GetComponentInChildren<InGameUI>();
            _settingsButton.onClick.AddListener(ShowSettingsScreen);

            //InGameUI.Hide();
        }

        // Events removed from LevelManager; UI should be triggered directly from LevelManager or other flows

        private void ShowWinPanel()
        {
            _winPanel.Open();
        }

        private void ShowLosePanel()
        {
            _failScreen.Open();
        }

        private void HideWinPanel()
        {
            _winPanel.Close();
        }

        private void HideLosePanel()
        {
            _failScreen.Close();
        }

        public void ShowSettingsScreen()
        {
            _settingsScreen.Open();
        }

        public void HideSettingsScreen()
        {
            _settingsScreen.Close();
        }

        // public void ShowLevelStartScreen()
        // {
        // 	_levelStartScreen.Open();
        // }

        private void ShowInGameUIInternal() { InGameUI?.Show(); }
        private void HideInGameUIInternal() { InGameUI?.Hide(); }

        private void UpdateLevelText()
        {
            _levelText.SetText($"Level {LevelManager.Instance.ActiveLevelNumber}");
        }

        // Public API for LevelManager to drive UI directly
        public void ShowWinScreen()
        {
            ShowWinPanel();
        }

        public void ShowFailScreen()
        {
            ShowLosePanel();
        }

        public void RefreshLevelNumber()
        {
            UpdateLevelText();
            InGameUI?.RefreshLevelNumber();
            // currency UI removed
        }
        

        // If you want to drive InGameUI visibility from LevelManager
        public void ShowInGameUI() { ShowInGameUIInternal(); }
        public void HideInGameUI() { HideInGameUIInternal(); }

        // Legacy event handlers removed; LevelManager drives UI directly
    }
}