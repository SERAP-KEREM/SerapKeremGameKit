using TMPro;
using UnityEngine;
using DG.Tweening;

namespace SerapKeremGameKit._Currency
{
    public sealed class CurrencyPopup : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI _text;
        [SerializeField] private float _riseDistance = 1f;
        [SerializeField] private float _duration = 0.8f;

        public void Play(int amount)
        {
            if (_text == null) return;
            _text.text = "+" + amount.ToString();

            Transform t = transform;
            Vector3 start = t.position;
            Vector3 end = start + Vector3.up * _riseDistance;
            t.position = start;
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 1f;

            Sequence s = DOTween.Sequence();
            s.Join(t.DOMove(end, _duration).SetEase(Ease.OutCubic))
             .Join(cg.DOFade(0f, _duration).SetEase(Ease.InCubic))
             .OnComplete(() => Destroy(gameObject));
        }
    }
}


