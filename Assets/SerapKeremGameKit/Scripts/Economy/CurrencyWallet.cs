using SerapKeremGameKit._Singletons;
using UnityEngine;

namespace SerapKeremGameKit._Economy
{
    public sealed class CurrencyWallet : MonoSingleton<CurrencyWallet>
    {
        private const string CoinsKey = "skgk.currency.coins.v1";
        private const string CoinsSigKey = "skgk.currency.coins.sig.v1";
        private const int Salt = 739391;

        public int Coins { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            Load();
        }

        public void Load()
        {
            int saved = PlayerPrefs.GetInt(CoinsKey, 0);
            int sig = PlayerPrefs.GetInt(CoinsSigKey, 0);
            Coins = sig == ComputeSig(saved) ? saved : 0;
        }

        public void Save()
        {
            PlayerPrefs.SetInt(CoinsKey, Coins);
            PlayerPrefs.SetInt(CoinsSigKey, ComputeSig(Coins));
            PlayerPrefs.Save();
        }

        public void Add(int amount)
        {
            if (amount <= 0) return;
            Coins += amount;
            Save();
        }

        public bool TrySpend(int amount)
        {
            if (amount <= 0) return true;
            if (Coins < amount) return false;
            Coins -= amount;
            Save();
            return true;
        }

        private int ComputeSig(int value)
        {
            unchecked { return (value ^ Salt) * 397; }
        }
    }
}



