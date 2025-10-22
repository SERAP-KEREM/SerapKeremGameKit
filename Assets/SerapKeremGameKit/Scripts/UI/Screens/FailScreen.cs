using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public sealed class FailScreen : UIScreen
    {
        [SerializeField] private Image _failIcon;
        [SerializeField] private TextMeshProUGUI _coinText;
        [SerializeField] private Button _restartButton;
        [SerializeField] private UIRootController _uiRoot;

        private void Awake()
        {
            if (_restartButton != null) _restartButton.onClick.AddListener(OnRestartClicked);
        }

        private void OnDestroy()
        {
            if (_restartButton != null) _restartButton.onClick.RemoveListener(OnRestartClicked);
        }

        public void Setup(int rewardedCoins, UIRootController uiRoot)
        {
            if (_coinText != null) _coinText.text = rewardedCoins.ToString();
            _uiRoot = uiRoot;
        }

        private void OnRestartClicked()
        {
            if (_uiRoot == null) _uiRoot = FindObjectOfType<UIRootController>(true);
            if (_uiRoot != null) _uiRoot.OnRestartConfirmed();
        }
    }
}



