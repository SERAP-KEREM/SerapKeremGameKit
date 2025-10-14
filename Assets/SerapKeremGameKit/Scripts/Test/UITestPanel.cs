using TriInspector;
using UnityEngine;

namespace SerapKeremGameKit._UI
{
    public sealed class UITestPanel : MonoBehaviour
    {
        [Title("References")]
        [SerializeField] private FeatureScreen _featureScreen;

        [Title("Feature Test Data")]
        [SerializeField] private string _featureTitle = "New Feature";
        [SerializeField] private string _featureDescription = "Description...";
        [SerializeField] private Sprite _featureSprite;

        private const string CoinsKey = "skgk.currency.coins";

        [Title("UI Tests")]
        [Button("Show InGame UI")]
        private void Btn_ShowInGame() { UIManager.Instance?.ShowInGameUI(); }

        [Button("Hide InGame UI")]
        private void Btn_HideInGame() { UIManager.Instance?.HideInGameUI(); }

        [Button("Show Win Screen")]
        private void Btn_ShowWin() { UIManager.Instance?.ShowWinScreen(); }

        [Button("Show Fail Screen")]
        private void Btn_ShowFail() { UIManager.Instance?.ShowFailScreen(); }

        [Button("Show Settings")]
        private void Btn_ShowSettings() { UIManager.Instance?.ShowSettingsScreen(); }

        [Button("Hide Settings")]
        private void Btn_HideSettings() { UIManager.Instance?.HideSettingsScreen(); }

        [Button("Refresh Level UI")]
        private void Btn_RefreshLevel() { UIManager.Instance?.RefreshLevelNumber(); }

        [Title("Feature Screen Tests")]
        [Button("Show Feature Screen")]
        private void Btn_ShowFeature()
        {
            if (_featureScreen != null)
            {
                _featureScreen.ShowFeature(_featureTitle, _featureDescription, _featureSprite);
            }
        }

        [Title("Currency Quick Test")]
        [SerializeField] private int _testCoinsToAdd = 10;

        [Button("Add Coins (PlayerPrefs)")]
        private void Btn_AddCoins()
        {
            int coins = PlayerPrefs.GetInt(CoinsKey, 0) + _testCoinsToAdd;
            PlayerPrefs.SetInt(CoinsKey, coins);
            PlayerPrefs.Save();
            UIManager.Instance?.RefreshLevelNumber();
        }
    }
}


