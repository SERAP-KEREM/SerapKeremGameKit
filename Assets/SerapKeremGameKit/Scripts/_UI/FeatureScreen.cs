using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using SerapKeremGameKit._Audio;


namespace SerapKeremGameKit._UI
{
    public class FeatureScreen : UIScreen
    {
        [SerializeField] private TextMeshProUGUI _featureTitle;
        [SerializeField] private TextMeshProUGUI _featureDescription;
        [SerializeField] private Image _featureImage;
        [SerializeField] private Button _closeButton;

        [SerializeField] private float _fadeDuration = 0.3f;

        private Coroutine _showCoroutine;

        protected override void Awake()
        {
            base.Awake();
            _closeButton.onClick.AddListener(OnCloseButtonClicked);
        }

        [Header("Audio")]
        [SerializeField] private string _openSfxKey;
        public void ShowFeature(string title, string description, Sprite featureSprite)
        {
            _featureTitle.text = title;
            _featureDescription.text = description;
            _featureImage.sprite = featureSprite;

            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);

            _featureImage.color = new Color(_featureImage.color.r, _featureImage.color.g, _featureImage.color.b, 0f);
            gameObject.SetActive(true);

            Open();

            if (!string.IsNullOrEmpty(_openSfxKey))
            {
                AudioManager.Instance.Play(_openSfxKey);
            }

            _showCoroutine = StartCoroutine(ShowRoutine());
        }

        private IEnumerator ShowRoutine()
        {
            // Fade in image
            _featureImage.DOFade(1f, _fadeDuration).SetUpdate(true);
            yield return new WaitForSecondsRealtime(_fadeDuration);
        }

        private void OnCloseButtonClicked()
        {
            if (_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
                _showCoroutine = null;
            }

            _featureImage.DOFade(0f, _fadeDuration).SetUpdate(true)
                .OnComplete(Close);
        }
    }
}