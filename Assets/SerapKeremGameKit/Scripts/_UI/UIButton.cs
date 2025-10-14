using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using DG.Tweening;
using TriInspector;

namespace SerapKeremGameKit._UI
{
    [RequireComponent(typeof(RectTransform))]
    [AddComponentMenu("RKN/UI/Custom UIButton")]
    public class CustomUIButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler
    {
        [Title("Scale Settings")]
        [SerializeField, Min(0f)] private float _pressScale = 0.9f;
        [SerializeField, Min(0f)] private float _scaleDuration = 0.1f;
        [SerializeField] private Ease _scaleEase = Ease.OutQuad;

        [Title("Events")]
        [SerializeField] private UnityEvent _onClick;

        private RectTransform _rectTransform;
        private Tween _scaleTween;
        private Vector3 _originalScale;

        private void Awake()
        {
            _rectTransform = GetComponent<RectTransform>();
            _originalScale = _rectTransform.localScale;
        }

        public void OnPointerDown(PointerEventData eventData)
        {
            AnimateScale(_pressScale);
        }

        public void OnPointerUp(PointerEventData eventData)
        {
            AnimateScale(_originalScale.x);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            _onClick?.Invoke();
        }

        private void AnimateScale(float targetScale)
        {
            _scaleTween?.Kill();
            _scaleTween = _rectTransform
                .DOScale(Vector3.one * targetScale, _scaleDuration)
                .SetEase(_scaleEase);
        }

#if UNITY_EDITOR
        // Reset komponent eklenince veya "Reset" yap�ld���nda �al���r
        private void Reset()
        {
            _rectTransform = GetComponent<RectTransform>();
        }
#endif
    }
}
