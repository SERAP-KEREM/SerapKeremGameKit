using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
	public sealed class HUDPanel : UIPanel
    {
        [SerializeField] private TextMeshProUGUI _levelText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private Button _settingsButton;
        [SerializeField] private UIRootController _uiRoot;

		private void Awake()
		{
			if (_restartButton != null) _restartButton.BindOnClick(this, OnRestartClicked);
			if (_settingsButton != null) _settingsButton.BindOnClick(this, OnSettingsClicked);
		}

		protected override void OnDestroy()
		{
			base.OnDestroy();
			// Auto-unsubscribe handled by ButtonExtensions
		}

        public void SetLevelIndex(int levelIndex)
        {
            if (_levelText != null)
                _levelText.text = $"Level {levelIndex + 1}";
        }

        private void OnRestartClicked()
        {
            if (_uiRoot != null) _uiRoot.OnRestartRequested();
        }

        private void OnSettingsClicked()
        {
            if (_uiRoot != null) _uiRoot.OnOpenSettings();
        }

		public void SetUIRoot(UIRootController uiRoot)
		{
			_uiRoot = uiRoot;
		}
    }
}



