using UnityEngine;
using UnityEngine.Events;

namespace SerapKeremGameKit._CurrencySystem
{
	public abstract class Currency
	{
		public abstract long Amount { get; set; }

		public event UnityAction<long, Vector3?, bool> OnAmountAdded;
		public event UnityAction<long, Vector3, float> OnAmountAddedFloating;
		public event UnityAction<long> OnAmountSpent;
		public event UnityAction OnNotEnoughCurrency;

		public void Init()
		{
			OnAmountAdded?.Invoke(0, default, default);
		}

		public virtual void AddCurrency(long amount, Vector3? position = null, bool isWorldPosition = true)
		{
			Amount += amount;
			OnAmountAdded?.Invoke(amount, position, isWorldPosition);
		}

		public virtual void AddCurrencyFloating(long amount, Vector3 position, float height = 0)
		{
			Amount += amount;
			OnAmountAddedFloating?.Invoke(amount, position, height);
		}

		public virtual void SpendCurrency(long amount)
		{
			Amount -= amount;
			OnAmountSpent?.Invoke(amount);
		}

		public bool IsEnough(long amount)
		{
			var isEnough = Amount >= amount;
			if (!isEnough)
				OnNotEnoughCurrency?.Invoke();

			return isEnough;
		}
	}
}