using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using TriInspector;
using SerapKeremGameKit._Singletons;

namespace SerapKeremGameKit._UI
{
    public class LoadingScreen : MonoSingleton<LoadingScreen>
    {
        [SerializeField, ReadOnly] private bool _isLoadingActiveDebug;
        public static bool IsLoadingActive { get; private set; }

        private void Update()
        {
            _isLoadingActiveDebug = IsLoadingActive;
        }
        public static UnityAction OnLoadingCompleted;

        [Header("Loading Settings")]
        [SerializeField] private float _loadingDuration = 3f;
        [SerializeField] private float _randomizeMultiplier = 1f;
        [SerializeField] private Ease _loadingEase = Ease.InOutQuad;

        [Header("UI References")]
        [SerializeField] private Image _progressBarImage;
        [SerializeField] private GameObject _loadingScreenContainer;
        [SerializeField] private Image _backgroundImage;
        [SerializeField] private Image _mainLoadingImage;
        [SerializeField] private Image _titleImage;
        [SerializeField] private Image _blinkingIndicatorImage;

        [Header("Blinking Indicator Settings")]
        [SerializeField] private float _blinkDuration = 0.5f;
        [SerializeField] private float _minAlpha = 0f;
        [SerializeField] private float _maxAlpha = 1f;
        [SerializeField] private Ease _blinkEase = Ease.InOutSine;

        private Tween _blinkingTween;

        protected override void Awake()
        {
            base.Awake();
            StartLoadingSequence();
        }

        private void StartLoadingSequence()
        {
            IsLoadingActive = true;
            _progressBarImage.fillAmount = 0f;
            _loadingScreenContainer.SetActive(true);

            StartBlinkingIndicator();

            float randomizedDuration = _loadingDuration + Random.Range(0f, _randomizeMultiplier);

            _progressBarImage.DOFillAmount(1f, randomizedDuration)
                .SetEase(_loadingEase)
                .SetLink(gameObject)
                .SetTarget(gameObject)
                .OnComplete(CompleteLoading);
        }

        private void StartBlinkingIndicator()
        {
            if (_blinkingIndicatorImage == null) return;

            _blinkingTween = _blinkingIndicatorImage.DOFade(_minAlpha, _blinkDuration)
                .From(_maxAlpha)
                .SetEase(_blinkEase)
                .SetLoops(-1, LoopType.Yoyo)
                .SetLink(gameObject);
        }

        private void CompleteLoading()
        {
            StopBlinkingIndicator();
            _loadingScreenContainer.SetActive(false);
            IsLoadingActive = false;
            OnLoadingCompleted?.Invoke();
        }

        private void StopBlinkingIndicator()
        {
            _blinkingTween?.Kill();
        }

        public void SetLoadingScreenVisuals(Sprite background, Sprite mainImage, Sprite titleImage)
        {
            if (background != null) _backgroundImage.sprite = background;
            if (mainImage != null) _mainLoadingImage.sprite = mainImage;
            if (titleImage != null) _titleImage.sprite = titleImage;
        }
    }
}