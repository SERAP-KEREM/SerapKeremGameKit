using SerapKeremGameKit._Economy;
using SerapKeremGameKit._LevelSystem;
using SerapKeremGameKit._Levels;
using SerapKeremGameKit._Managers;
using UnityEngine;

namespace SerapKeremGameKit._UI
{
    public sealed class UIRootController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UIManagerHUD _hud;
        [SerializeField] private WinScreen _win;
        [SerializeField] private FailScreen _fail;
        [SerializeField] private SettingsScreen _settings;
        [SerializeField] private RetryScreen _retry;

        [Header("Data")] 
        [SerializeField] private LevelConfig _fallbackConfig;

        private GameState _lastState = GameState.None;

        private void Awake()
        {
            // Auto-wire if not assigned
            if (_hud == null) _hud = GetComponentInChildren<UIManagerHUD>(true);
            if (_win == null) _win = GetComponentInChildren<WinScreen>(true);
            if (_fail == null) _fail = GetComponentInChildren<FailScreen>(true);
            if (_settings == null) _settings = GetComponentInChildren<SettingsScreen>(true);
            if (_retry == null) _retry = GetComponentInChildren<RetryScreen>(true);

            // Ensure startup state: only HUD hidden initially (will be shown in Start)
            if (_win != null) _win.HideImmediate();
            if (_fail != null) _fail.HideImmediate();
            if (_settings != null) _settings.HideImmediate();
            if (_retry != null) _retry.HideImmediate();
            if (_hud != null) _hud.HideImmediate();
        }

        private void Start()
        {
            ApplyInitialState();
        }

        private void Update()
        {
            SyncWithGameState();
        }

        private void ApplyInitialState()
        {
            HideAll();
            if (_hud != null)
            {
                _hud.Show();
                _hud.SetLevelIndex(LevelManager.Instance.ActiveLevelNumber - 1);
            }
        }

        private void SyncWithGameState()
        {
            GameState current = StateManager.Instance.CurrentState;
            if (current == _lastState) return;
            _lastState = current;

            if (current == GameState.OnStart)
            {
                HideAll();
                if (_hud != null)
                {
                    _hud.Show();
                    _hud.SetLevelIndex(LevelManager.Instance.ActiveLevelNumber - 1);
                }
            }
            else if (current == GameState.OnWin)
            {
                ShowWin();
            }
            else if (current == GameState.OnLose)
            {
                ShowFail();
            }
        }

        private void HideAll()
        {
            if (_hud != null) _hud.Hide();
            if (_win != null) _win.Hide();
            if (_fail != null) _fail.Hide();
            if (_settings != null) _settings.Hide();
            if (_retry != null) _retry.Hide();
        }

        private void ShowWin()
        {
            HideExcept(_win);
            if (_win != null)
            {
                Level active = LevelManager.Instance.ActiveLevelInstance;
                LevelConfig config = ResolveConfig(active);
                float timeSec = (float)StateManager.Instance.GetLevelTime();
                int stars = StarEvaluator.EvaluateStars(config, timeSec);
                int reward = Mathf.Max(0, config != null ? config.WinCoins : 10);
                int totalBefore = 0;
                if (CurrencyWallet.Instance != null)
                {
                    totalBefore = CurrencyWallet.Instance.Coins;
                }
                _win.Setup(stars, reward, totalBefore, this);
                _win.Show();
            }
        }

        private void ShowFail()
        {
            HideExcept(_fail);
            if (_fail != null)
            {
                Level active = LevelManager.Instance.ActiveLevelInstance;
                LevelConfig config = ResolveConfig(active);
                int reward = Mathf.Max(0, config != null ? config.FailCoins : 0);
                if (reward > 0 && CurrencyWallet.Instance != null) CurrencyWallet.Instance.Add(reward);
                _fail.Setup(reward, this);
                _fail.Show();
            }
        }

        private LevelConfig ResolveConfig(Level level)
        {
            if (level != null)
            {
                LevelConfig attached = level.GetComponent<LevelConfig>();
                if (attached != null) return attached;
            }
            return _fallbackConfig;
        }

        private void HideExcept(UIScreen screen)
        {
            if (_hud != null && _hud != screen) _hud.Hide();
            if (_win != null && _win != screen) _win.Hide();
            if (_fail != null && _fail != screen) _fail.Hide();
            if (_settings != null && _settings != screen) _settings.Hide();
            if (_retry != null && _retry != screen) _retry.Hide();
        }

        public void OnRestartRequested()
        {
            if (_retry != null) _retry.Show();
        }

        public void OnRestartConfirmed()
        {
            HideAll();
            LevelManager.Instance.RestartLevel();
        }

        public void OnNextLevelRequested()
        {
            HideAll();
            if (_hud != null)
            {
                _hud.Show();
            }
            LevelManager.Instance.IncreaseLevelNumber();
            LevelManager.Instance.LoadCurrentLevel();
        }

        public void ProceedNextLevelAfterReward(int reward)
        {
            if (CurrencyWallet.Instance != null && reward > 0)
            {
                CurrencyWallet.Instance.Add(reward);
            }
            OnNextLevelRequested();
        }

        public void OnOpenSettings()
        {
            if (_settings != null) _settings.Show();
        }
    }
}



