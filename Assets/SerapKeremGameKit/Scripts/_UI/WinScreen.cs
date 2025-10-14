using DG.Tweening;
using UnityEngine;
using SerapKeremGameKit._CurrencySystem;
using SerapKeremGameKit._Logging;
using SerapKeremGameKit._Managers;
using TMPro;
using TriInspector;
using UnityEngine.UI;
using SerapKeremGameKit._Feature;

namespace SerapKeremGameKit._UI
{
    public class WinScreen : UIScreen
    {
        [Title("Win Panel Settings")]
        [SerializeField] private Button _nextButton;
        [SerializeField] private TextMeshProUGUI _moneyAmountText;
        [SerializeField] private Transform _winTitleText;

        [Header("Coin Effect")]
        [SerializeField] private float _coinEffectDuration = 1f;

        [SerializeField] private GameObject _winScreenSecondPhase;
        [SerializeField] private GameObject _baseBackground;

        // Cached components
        private CanvasGroup _secondPhaseCanvasGroup;

        // Animation control
        private Sequence _winSequence;

        [Header("Feature Settings")]
        [SerializeField] private TextMeshProUGUI _featureTitleText;
        [SerializeField] private TextMeshProUGUI _featureDescriptionText;
        [SerializeField] private Image _featureImage;
        [SerializeField] private Slider _featureProgressSlider;
        [SerializeField] private FeatureManager _featureManager;

        protected override void Awake()
        {
            base.Awake();
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            _nextButton.onClick.AddListener(OnNextButtonClicked);

            // Cache CanvasGroup components
            _secondPhaseCanvasGroup = GetCanvasGroup(_winScreenSecondPhase);
        }

        private CanvasGroup GetCanvasGroup(GameObject obj)
        {
            CanvasGroup canvasGroup = obj.GetComponent<CanvasGroup>();
            return canvasGroup;
        }

        protected override void OnOpenStart()
        {
            base.OnOpenStart();
            SetupWinScreen();
        }

        protected override void OnCloseStart()
        {
            base.OnCloseStart();
            CleanupAnimations();
        }

        private void SetupWinScreen()
        {
            SetMoneyAmount();
            ResetScreenStates();
            SetupFeatureSection();
            StartWinSequence();
        }

        private void SetMoneyAmount()
        {
            _moneyAmountText.SetText($"+{LevelManager.Instance.ActiveLevelInstance.Money}");
        }

        private void ResetScreenStates()
        {
            _baseBackground.SetActive(true);
            _winScreenSecondPhase.SetActive(false);

            _secondPhaseCanvasGroup.alpha = 0f;

            _nextButton.interactable = false;
        }

        private void StartWinSequence()
        {
            _winSequence?.Kill();
            _winSequence = DOTween.Sequence();

            // Phase 2: Show win title and next button
            _winSequence.AppendCallback(() => SetupSecondPhase());
        }

        private void SetupSecondPhase()
        {
            _winScreenSecondPhase.SetActive(true);

            // Reset scales and animate
            _winTitleText.localScale = Vector3.zero;
            _nextButton.transform.localScale = Vector3.zero;

            var sequence = DOTween.Sequence();
            sequence.Append(_winTitleText.DOScale(1f, 0.75f).SetEase(Ease.OutBack));
            sequence.Join(_nextButton.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack));
            sequence.Join(_secondPhaseCanvasGroup.DOFade(1f, 0.5f));
            sequence.OnComplete(() => _nextButton.interactable = true);
        }

        private void SetupFeatureSection()
        {
            int currentLevel = LevelManager.Instance.ActiveLevelNumber;

            if (_featureManager != null && _featureManager.TryGetFeatureForUI(currentLevel,
                out string title, out string description, out Sprite sprite, out float progress))
            {
                _featureTitleText.text = title;
                _featureDescriptionText.text = description;
                _featureImage.sprite = sprite;

                // Kill old tweens before animating
                _featureProgressSlider.DOKill();

                // Animate slider smoothly
                _featureProgressSlider.DOValue(progress, 0.75f).SetEase(Ease.OutCubic);
            }
            else
            {
                _featureTitleText.text = string.Empty;
                _featureDescriptionText.text = string.Empty;
                _featureImage.sprite = null;

                _featureProgressSlider.DOKill();
                _featureProgressSlider.value = 0f;
            }
        }

        private void OnNextButtonClicked()
        {
            _nextButton.interactable = false;

            CurrencyManager.Money.AddCurrency(
               LevelManager.Instance.ActiveLevelInstance.Money,
               _moneyAmountText.rectTransform.position,
               false
           );

            // Delay before loading next level
            DOVirtual.DelayedCall(_coinEffectDuration, NextButtonDelayCall);
        }

        private void NextButtonDelayCall()
        {
            LevelManager.Instance.CleanCurrentLevel();
            LevelManager.Instance.IncreaseLevelNumber();
            LevelManager.Instance.StartCurrentLevelInstance();
            Close();
        }

        // private void ShowLevelStartScreen()
        // {
        //     // LevelManager.Instance.LoadNextLevel();
        //     LevelManager.Instance.CleanCurrentLevel();
        //     LevelManager.Instance.IncreaseLevelNumber();
        //     UIManager.Instance.ShowLevelStartScreen();
        //     Close();
        // }

        private void CleanupAnimations()
        {
            _winSequence?.Kill();
            transform.DOKill();

            ResetUIElements();
        }

        private void ResetUIElements()
        {
            _baseBackground.SetActive(false);
            _winScreenSecondPhase.SetActive(false);

            _secondPhaseCanvasGroup.alpha = 0f;
        }

        private void OnDestroy()
        {
            CleanupAnimations();
            _nextButton?.onClick.RemoveAllListeners();
        }
    }
}