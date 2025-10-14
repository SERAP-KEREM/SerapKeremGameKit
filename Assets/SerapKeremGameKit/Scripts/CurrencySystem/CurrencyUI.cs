using DG.Tweening;
using TMPro;
using UnityEngine;
using SerapKeremGameKit._Audio;
using SerapKeremGameKit._Haptics;

namespace SerapKeremGameKit._CurrencySystem
{
    public abstract class CurrencyUI : MonoBehaviour
    {
        [SerializeField] protected RectTransform _target;
        [SerializeField] protected TextMeshProUGUI _currecyText;
        [SerializeField] private string _coinSfxKey;
        private Currency _currency;
        private long _currentCurrencyAmount;

        private const float SPAWN_DURATION = .4F;
        private const float MOVE_DURATION = 1;
        private const float DELAY = .05F;

        protected virtual void OnEnable()
        {
            _currency.OnAmountAdded += AmountAdded;
            _currency.OnAmountAddedFloating += AmountAddedFloating;
            _currency.OnAmountSpent += AmountSpent;
            _currency.OnNotEnoughCurrency += NotEnoughCurrency;
        }

        protected virtual void OnDisable()
        {
            _currency.OnAmountAdded -= AmountAdded;
            _currency.OnAmountAddedFloating -= AmountAddedFloating;
            _currency.OnAmountSpent -= AmountSpent;
            _currency.OnNotEnoughCurrency -= NotEnoughCurrency;
        }

        protected virtual void OnDestroy()
        {
            DOTween.Complete("icon");
            DOTween.Complete("icon-spend");
            DOTween.Complete("icon-floating");
            _target.DOComplete();
        }

        public virtual void Init(Currency _currency)
        {
            this._currency = _currency;
            _currentCurrencyAmount = this._currency.Amount;

            SetCurrencyText(_currentCurrencyAmount);
        }

        protected void SetCurrencyText(long amount)
        {
            _currecyText.SetText(amount.ToString());
        }

        protected virtual void AmountAdded(long amount, Vector3? position = null, bool isWorldPosition = true)
        {
            if (position is null)
            {
                _currentCurrencyAmount += amount;
                SetCurrencyText(Mathf.CeilToInt(_currentCurrencyAmount));
            }
            else
            {
                if (isWorldPosition)
                {
                    position = Camera.main.WorldToScreenPoint((Vector3)position);
                }

                var tempCurrency = _currentCurrencyAmount + amount;
                var tempCurrentCurrency = _currentCurrencyAmount;

                int count = Mathf.Clamp((int)amount, 2, 10);
                for (int i = 0; i < count; i++)
                {
                    var icon = CoinPool.Instance.SpawnCoin((Vector3)position, Quaternion.identity);
                    icon.transform.SetParent(transform);
                    float i1 = i;
                    icon.transform.DOMove((Vector3)position + 100 * (Vector3)Random.insideUnitCircle, SPAWN_DURATION).SetDelay(i * DELAY).SetEase(Ease.InOutSine).SetId("icon").OnComplete(() =>
                    {
                        icon.transform.DOMove(_target.position, MOVE_DURATION).SetEase(Ease.InBack).OnComplete(() =>
                        {
                            tempCurrency = _currentCurrencyAmount + amount;

                            _target.DOComplete();
                            _target.DOPunchScale(.9f * Vector3.one, .2f, 2, .5f);
                            SetCurrencyText(Mathf.CeilToInt(Mathf.Lerp(tempCurrentCurrency, tempCurrency, i1 / (count - 1))));
                            if (!string.IsNullOrEmpty(_coinSfxKey))
                            {
                                AudioManager.Instance.Play(_coinSfxKey);
                            }

                            CoinPool.Instance.ReturnCoin(icon);
                            HapticManager.Instance.Play(HapticType.Light);
                        });
                    });
                }

                DOVirtual.DelayedCall(count * DELAY + SPAWN_DURATION + MOVE_DURATION, () => _currentCurrencyAmount = tempCurrency);
            }
        }

        protected virtual void AmountAddedFloating(long amount, Vector3 position, float height)
        {
            // long tempCurrency = currentCurrencyAmount + amount;

            // var icon = ObjectPooler.Instance.Spawn(currencyFloatingPoolName, position).GetComponent<FloatingText>();
            // icon.Setup(amount);
            // icon.Float(position + height * Vector3.up, currencyFloatingPoolName);

            // txt_Currency.SetText(Mathf.CeilToInt(tempCurrency).ToString());
            // target.DOComplete();
            // target.DOPunchScale(Vector3.one * .9f, .2f, 2, .5f);

            // currentCurrencyAmount = tempCurrency;
        }

        protected virtual void AmountSpent(long amount)
        {
            DOTween.Complete("icon-spend");
            long tempCurrency = _currentCurrencyAmount - amount;
            DOTween.To(() => _currentCurrencyAmount, x => _currentCurrencyAmount = x, tempCurrency, 1).SetEase(Ease.OutCubic)
                .OnUpdate(() => _currecyText.SetText(Mathf.CeilToInt(_currentCurrencyAmount).ToString())).OnComplete(() =>
                {
                    _currentCurrencyAmount = tempCurrency;
                    _currecyText.SetText(_currentCurrencyAmount.ToString());
                }).SetId("icon-spend");
        }

        protected virtual void NotEnoughCurrency()
        {
            // HapticManager.Instance.PlayHaptic(HapticPatterns.PresetType.Warning);

            _currecyText.DOComplete();
            _currecyText.transform.DOComplete();
            _currecyText.transform.DOScale(1.5f, .1f).SetLoops(2, LoopType.Yoyo);
            _currecyText.DOColor(Color.red, .1f).SetLoops(2, LoopType.Yoyo);
        }
    }
}