using UnityEngine;
using System.Collections;
using SerapKeremGameKit._Logging;

namespace SerapKeremGameKit._UI
{
    [RequireComponent(typeof(CanvasGroup))]
    public abstract class UIScreen : MonoBehaviour
    {
        [Header("Animation Settings")]
        [SerializeField] private float _animationDuration = 0.3f;
        [SerializeField] private AnimationCurve _easeCurve = AnimationCurve.EaseInOut(0, 0, 1, 1);

        [Header("Startup Settings")]
        [SerializeField] private bool _isOpenOnStart = false;

        private CanvasGroup _canvasGroup;
        private Coroutine _currentAnimation;

        public bool IsOpen { get; private set; }
        public bool IsAnimating { get; private set; }

        protected virtual void Awake()
        {
            _canvasGroup = GetComponent<CanvasGroup>();
        }

        protected virtual void Start()
        {
            if (_isOpenOnStart)
            {
                gameObject.SetActive(true);
                _canvasGroup.alpha = 1f;
                _canvasGroup.interactable = true;
                IsOpen = true;
            }
            else
            {
                gameObject.SetActive(false);
                _canvasGroup.alpha = 0f;
                _canvasGroup.interactable = false;
                IsOpen = false;
            }
        }

        public virtual void Open()
        {
            if (IsOpen || IsAnimating)
                return;

            // RichLogger.Log("OPEN!");

            gameObject.SetActive(true);

            if (_currentAnimation != null)
                StopCoroutine(_currentAnimation);

            _currentAnimation = StartCoroutine(FadeIn());

            OnOpenStart();
        }

        public virtual void Close()
        {
            if (!IsOpen || IsAnimating)
                return;

            if (_currentAnimation != null)
                StopCoroutine(_currentAnimation);

            _currentAnimation = StartCoroutine(FadeOut());

            OnCloseStart();
        }

        private IEnumerator FadeIn()
        {
            IsAnimating = true;
            _canvasGroup.interactable = false;

            float elapsedTime = 0f;
            float startAlpha = _canvasGroup.alpha;

            while (elapsedTime < _animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / _animationDuration;
                float easedProgress = _easeCurve.Evaluate(progress);

                _canvasGroup.alpha = Mathf.Lerp(startAlpha, 1f, easedProgress);
                yield return null;
            }

            _canvasGroup.alpha = 1f;
            _canvasGroup.interactable = true;
            IsOpen = true;
            IsAnimating = false;

            OnOpenComplete();
        }

        private IEnumerator FadeOut()
        {
            IsAnimating = true;
            _canvasGroup.interactable = false;

            float elapsedTime = 0f;
            float startAlpha = _canvasGroup.alpha;

            while (elapsedTime < _animationDuration)
            {
                elapsedTime += Time.unscaledDeltaTime;
                float progress = elapsedTime / _animationDuration;
                float easedProgress = _easeCurve.Evaluate(progress);

                _canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, easedProgress);
                yield return null;
            }

            _canvasGroup.alpha = 0f;
            gameObject.SetActive(false);
            IsOpen = false;
            IsAnimating = false;

            OnCloseComplete();
        }

        protected virtual void OnOpenStart() { }
        protected virtual void OnOpenComplete() { }
        protected virtual void OnCloseStart() { }
        protected virtual void OnCloseComplete() { }
    }
}
