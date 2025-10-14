using UnityEngine;

namespace SerapKeremGameKit._CurrencySystem
{
    public class MoneyUI : CurrencyUI
    {
        protected override void OnEnable()
        {
            Init(CurrencyManager.Money);

            base.OnEnable();
        }
    }
}