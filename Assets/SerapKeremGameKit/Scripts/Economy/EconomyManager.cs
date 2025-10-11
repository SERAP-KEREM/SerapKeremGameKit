using SerapKeremGameKit._Logging;
using SerapKeremGameKit._Singletons;
using UnityEngine;

namespace SerapKeremGameKit._Economy
{
    public sealed class EconomyManager : MonoSingleton<EconomyManager>
    {
        private const string CoinsKey = "skgk.economy.coins";

        [SerializeField] private int _startCoins = 0;

        public int Coins { get; private set; }

        protected override void Awake()
        {
            base.Awake();
            if (Instance != this) return;
            Load();
        }

        public void AddCoins(int amount)
        {
            if (amount <= 0) return;
            Coins += amount;
            Save();
        }

        public bool SpendCoins(int amount)
        {
            if (amount <= 0) return true;
            if (Coins < amount) return false;
            Coins -= amount;
            Save();
            return true;
        }


        private void Load()
        {
            Coins = PlayerPrefs.GetInt(CoinsKey, _startCoins);
            // coins-only economy; no money load
        }

        private void Save()
        {
            PlayerPrefs.SetInt(CoinsKey, Coins);
            // coins-only economy; no money save
            PlayerPrefs.Save();
        }
    }
}


