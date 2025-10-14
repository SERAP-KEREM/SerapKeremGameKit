using SerapKeremGameKit._LevelSystem;
using SerapKeremGameKit._Singletons;
using TriInspector;
using UnityEngine;
using SerapKeremGameKit._Logging;
using SerapKeremGameKit._UI;

namespace SerapKeremGameKit._Managers
{
    [DefaultExecutionOrder(-2)]
    public class LevelManager : MonoSingleton<LevelManager>
    {
        #region Properties & Data Access

        public int ActiveLevelNumber
        {
            get => PlayerPrefs.GetInt("ProgressData", 1);
            set => PlayerPrefs.SetInt("ProgressData", value);
        }

        [Tooltip("Enable randomized level selection after tutorial completion.")]
        [SerializeField] private bool _enableRandomizedSelection = true;

        [Title("Level Collections")]
        [ListDrawerSettings(Draggable = true, AlwaysExpanded = false)]
        [SerializeField, Required] private Level[] _gameplayLevels;

        [SerializeField] private Level[] _introductionLevels;

        public Level ActiveLevelInstance { get; private set; }
        public int ProcessedLevelIndex { get; private set; }

        // Public accessors for external systems
        public Level[] GameplayLevels => _gameplayLevels;
        public Level[] TutorialLevels => _introductionLevels;

        #endregion

        // Events removed to keep template simple and robust

        #region Unity Lifecycle

        protected override void Awake()
        {
            base.Awake();
            PerformInitialValidation();
        }

        void Start()
        {
            StartCurrentLevelInstance();
        }

        public void StartCurrentLevelInstance()
        {
            ConfigureEnvironment();
            LoadCurrentLevel();
        }

        #endregion

        #region Core Level Management

        public void LoadCurrentLevel()
        {
            var levelConfiguration = DetermineLevelConfiguration();
            ProcessedLevelIndex = levelConfiguration.targetIndex;
            ExecuteLevelInstantiation(levelConfiguration.selectedLevel);
        }

        private (Level selectedLevel, int targetIndex) DetermineLevelConfiguration()
        {
            int introductionCount = _introductionLevels.Length;
            int currentProgress = ActiveLevelNumber;

            if (IsWithinIntroductionRange(currentProgress, introductionCount))
            {
                return (_introductionLevels[currentProgress - 1], currentProgress);
            }

            return ProcessGameplayLevelSelection(currentProgress - introductionCount);
        }

        private bool IsWithinIntroductionRange(int progress, int totalIntroLevels)
        {
            return progress <= totalIntroLevels;
        }

        private (Level selectedLevel, int targetIndex) ProcessGameplayLevelSelection(int adjustedProgress)
        {
            int totalGameplayLevels = _gameplayLevels.Length;
            int calculatedIndex = DetermineGameplayLevelIndex(adjustedProgress, totalGameplayLevels);

            return (_gameplayLevels[calculatedIndex - 1], calculatedIndex);
        }

        private int DetermineGameplayLevelIndex(int progressValue, int totalAvailable)
        {
            if (progressValue <= totalAvailable)
                return progressValue;

            if (_enableRandomizedSelection)
                return GenerateRandomLevelIndex(totalAvailable);

            return CalculateWrappedIndex(progressValue, totalAvailable);
        }

        private int GenerateRandomLevelIndex(int maxRange) => Random.Range(1, maxRange + 1);

        private int CalculateWrappedIndex(int value, int wrapLimit)
        {
            int remainder = value % wrapLimit;
            return remainder == 0 ? wrapLimit : remainder;
        }

        private void ExecuteLevelInstantiation(Level targetLevel)
        {
            ActiveLevelInstance = Instantiate(targetLevel);
            ActiveLevelInstance.Load();
            StateManager.Instance.SetLoading();
            // UI: update level text on load
            UIManager.Instance?.RefreshLevelNumber();
            StartLevel();
        }

        #endregion

        #region Level Control Methods

        public void StartLevel()
        {
            ActiveLevelInstance.Play();
            StateManager.Instance.SetOnStart();
            // UI: show in-game UI and refresh text
            UIManager.Instance?.ShowInGameUI();
            UIManager.Instance?.RefreshLevelNumber();
        }

        public void RetryLevel()
        {
            TerminateCurrentLevel();
            var retryTarget = _gameplayLevels[ProcessedLevelIndex - 1];
            ExecuteLevelInstantiation(retryTarget);
        }

        public void RestartLevel()
        {
            StateManager.Instance.SetOnRestart();
            RetryLevel();
        }

        public void CleanCurrentLevel()
        {
            TerminateCurrentLevel();
            // UI: hide gameplay UI if needed
            UIManager.Instance?.HideInGameUI();
        }

        public void IncreaseLevelNumber()
        {
            TerminateCurrentLevel();
            ActiveLevelNumber++;
        }

        private void TerminateCurrentLevel()
        {
            // notify state if needed
            if (ActiveLevelInstance != null)
                Destroy(ActiveLevelInstance.gameObject);
        }

        #endregion

        #region Game Result Handlers
        [Button("Test LevelWin")]
        public void Win()
        {
            if (!ValidateGameStateForEvents()) return;
            StateManager.Instance.SetOnWin();
            // Example: level-based coin reward
            // Currency.Currency.Add(ActiveLevelNumber * 5);
            // UI: show win panel
            UIManager.Instance?.ShowWinScreen();
        }

        [Button("Test LevelWin")]
        public void Win(int moveCount)
        {
            if (!ValidateGameStateForEvents()) return;
            StateManager.Instance.SetOnWin();
            // Example: move-based bonus
            // int bonus = Mathf.Max(0, 10 - moveCount);
            // Currency.Currency.Add(bonus);
        }

        [Button("Test LevelLose")]
        public void Lose()
        {
            if (!ValidateGameStateForEvents()) return;
            StateManager.Instance.SetOnLose();
            // UI: show fail panel
            UIManager.Instance?.ShowFailScreen();
        }

        private bool ValidateGameStateForEvents()
        {
            return StateManager.Instance.CurrentState == GameState.OnStart;
        }

        #endregion

        #region Utility & Validation Methods

        private void PerformInitialValidation()
        {
            if (_gameplayLevels == null || _gameplayLevels.Length == 0)
                Debug.LogWarning($"{name}: Gameplay levels collection is not configured properly.", this);
        }

        private void ConfigureEnvironment()
        {
#if UNITY_EDITOR
            CleanupExistingLevelsInEditor();
#endif
        }

#if UNITY_EDITOR
        private void CleanupExistingLevelsInEditor()
        {
            var existingLevelInstances = FindObjectsByType<Level>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var levelInstance in existingLevelInstances)
                levelInstance.gameObject.SetActive(false);
        }
#endif

        #endregion

        public Level GetLevelByNumber(int levelNumber)
        {
            int introCount = _introductionLevels.Length;

            if (levelNumber <= introCount)
            {
                if (levelNumber <= 0 || levelNumber > _introductionLevels.Length) return null;
                return _introductionLevels[levelNumber - 1];
            }

            int gameplayIndex = levelNumber - introCount;

            if (gameplayIndex <= 0 || gameplayIndex > _gameplayLevels.Length) return null;

            return _gameplayLevels[gameplayIndex - 1];
        }

        #region Utility & Validation Methods
        public Level GetCurrentLevel()
        {
            return GetLevelByNumber(ActiveLevelNumber);
        }

        public Level GetNextLevel()
        {
            return GetLevelByNumber(ActiveLevelNumber + 1) ?? GetLevelByNumber(1);
        }

        public Level GetNextestLevel()
        {
            return GetLevelByNumber(ActiveLevelNumber + 2) ?? GetLevelByNumber(1);
        }

        public Level GetFinalLevel()
        {
            return GetLevelByNumber(ActiveLevelNumber + 3) ?? GetLevelByNumber(1);
        }

        public Level GetFinalNextLevel()
        {
            return GetLevelByNumber(ActiveLevelNumber + 4) ?? GetLevelByNumber(1);
        }
        #endregion
    }
}