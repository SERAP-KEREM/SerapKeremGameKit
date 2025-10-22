using DG.Tweening;
using UnityEngine;

namespace SerapKeremGameKit._UI
{
    public abstract class UIScreen : MonoBehaviour
    {
        [SerializeField] protected CanvasGroup canvasGroup;
        [SerializeField] protected float showDuration = 0.2f;
        [SerializeField] protected float hideDuration = 0.15f;
        [SerializeField] protected Ease showEase = Ease.OutQuad;
        [SerializeField] protected Ease hideEase = Ease.OutQuad;

        private Tween _fadeTween;

        public virtual void Show()
        {
            gameObject.SetActive(true);
            _fadeTween?.Kill();
            if (canvasGroup == null) return;
            canvasGroup.alpha = 0f;
            _fadeTween = canvasGroup.DOFade(1f, showDuration).SetEase(showEase).SetUpdate(true);
        }

        public virtual void Hide()
        {
            _fadeTween?.Kill();
            if (canvasGroup == null)
            {
                gameObject.SetActive(false);
                return;
            }
            _fadeTween = canvasGroup.DOFade(0f, hideDuration).SetEase(hideEase).SetUpdate(true).OnComplete(() =>
            {
                gameObject.SetActive(false);
            });
        }

        protected virtual void OnDestroy()
        {
            _fadeTween?.Kill();
        }

        public void HideImmediate()
        {
            _fadeTween?.Kill();
            if (canvasGroup != null)
            {
                canvasGroup.alpha = 0f;
            }
            gameObject.SetActive(false);
        }
    }
}



