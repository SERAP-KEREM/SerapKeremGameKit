using System;
using DG.Tweening;
using SerapKeremGameKit._InputSystem;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public class RetryScreen : UIScreen
    {
        [Space]
        [SerializeField] private RectTransform _retryBoxPanel;

        [Header("Texts")]
        [SerializeField] private TextMeshProUGUI _titleText;

        [Header("Buttons")]
        [SerializeField] private Button _yesButton;
        [SerializeField] private Button _noButton;
        [SerializeField] private Button _backgroundButton;

        [Header("Custom Animations")]
        [SerializeField] private float _boxAnimationDuration = 0.5f;
        [SerializeField] private Ease _showEase = Ease.OutBack;
        [SerializeField] private Ease _hideEase = Ease.InBack;

        private Action _onTrueCallback;
        private Action _onFalseCallback;
        private Tween _boxTween;

        protected override void Awake()
        {
            base.Awake();
            InitializeButtons();
        }

        private void InitializeButtons()
        {
            _yesButton.onClick.AddListener(OnYesClicked);
            _noButton.onClick.AddListener(OnNoClicked);
            _backgroundButton.onClick.AddListener(OnNoClicked); // Background click = No
        }

        public void Show(string message, string title, Action onTrue = null, Action onFalse = null)
        {
            SetupContent(message, title);
            SetupCallbacks(onTrue, onFalse);
            Open();
        }

        private void SetupContent(string message, string title)
        {
            _titleText.text = title;
        }

        private void SetupCallbacks(Action onTrue, Action onFalse)
        {
            _onTrueCallback = onTrue;
            _onFalseCallback = onFalse;
        }

        protected override void OnOpenStart()
        {
            base.OnOpenStart();
            PrepareForShow();
            AnimateBoxIn();
        }

        protected override void OnCloseStart()
        {
            base.OnCloseStart();
            AnimateBoxOut();
        }

        private void PrepareForShow()
        {
            InputHandler.Instance?.LockInput();
            
            SetupButtons();
            _backgroundButton.gameObject.SetActive(true);
            _retryBoxPanel.gameObject.SetActive(true);
            _retryBoxPanel.localScale = Vector3.zero;
        }

        private void SetupButtons()
        {
            _yesButton.gameObject.SetActive(true);
            _noButton.gameObject.SetActive(true);
            
            // Disable buttons during animation
            _yesButton.interactable = false;
            _noButton.interactable = false;
        }

        private void AnimateBoxIn()
        {
            _boxTween?.Kill();
            _boxTween = _retryBoxPanel
                .DOScale(1f, _boxAnimationDuration)
                .SetEase(_showEase)
                .OnComplete(() =>
                {
                    // Enable buttons after animation
                    _yesButton.interactable = true;
                    _noButton.interactable = true;
                });
        }

        private void AnimateBoxOut()
        {
            InputHandler.Instance?.UnlockInput();
            _backgroundButton.gameObject.SetActive(false);
            
            // Disable buttons during animation
            _yesButton.interactable = false;
            _noButton.interactable = false;

            _boxTween?.Kill();
            _boxTween = _retryBoxPanel
                .DOScale(0f, _boxAnimationDuration)
                .SetEase(_hideEase)
                .OnComplete(() =>
                {
                    _retryBoxPanel.gameObject.SetActive(false);
                    ClearCallbacks();
                });
        }

        private void OnYesClicked()
        {
            if (!_yesButton.interactable) return;
            
            var callback = _onTrueCallback;
            Close();
            callback?.Invoke();
        }

        private void OnNoClicked()
        {
            if (!_noButton.interactable) return;
            
            var callback = _onFalseCallback;
            Close();
            callback?.Invoke();
        }

        private void ClearCallbacks()
        {
            _onTrueCallback = null;
            _onFalseCallback = null;
        }

        private void OnDestroy()
        {
            _boxTween?.Kill();
            
            _yesButton?.onClick.RemoveAllListeners();
            _noButton?.onClick.RemoveAllListeners();
            _backgroundButton?.onClick.RemoveAllListeners();
            
            ClearCallbacks();
        }
    }
}