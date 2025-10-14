using DG.Tweening;
using SerapKeremGameKit._Managers;
using UnityEngine;
using UnityEngine.UI;

namespace SerapKeremGameKit._UI
{
    public class FailScreen : UIScreen
    {
        [SerializeField] private Button _retryButton;
        [SerializeField] private Transform _failTitleText;

        protected override void Awake()
        {
            base.Awake();
            _retryButton.onClick.AddListener(Retry);
        }

        private void Retry()
        {
            Close();
        }
        protected override void OnCloseStart()
        {
            base.OnCloseStart();
            CloseFailScreenElements();
            LevelManager.Instance.RetryLevel();
        }
        private void CloseFailScreenElements()
        {
            _retryButton.gameObject.transform.DOKill();
            _failTitleText.gameObject.transform.DOKill();
        }

        public override void Open()
        {
            base.Open();
        }
        protected override void OnOpenStart()
        {
            base.OnOpenStart();
            OpenFailScreenElements();
        }
        private void OpenFailScreenElements()
        {
            _retryButton.gameObject.transform.localScale = Vector3.zero;
            _failTitleText.gameObject.transform.localScale = Vector3.zero;

            _retryButton.gameObject.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack);
            _failTitleText.gameObject.transform.DOScale(1f, 0.75f).SetEase(Ease.OutBack);
        }
    }
}