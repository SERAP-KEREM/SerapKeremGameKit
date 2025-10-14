using SerapKeremGameKit._Singletons;
using System.Collections.Generic;
using UnityEngine;

namespace SerapKeremGameKit._CurrencySystem
{
    public class CoinPool : MonoSingleton<CoinPool>
    {
        [SerializeField] private GameObject _coinPrefab;
        [SerializeField] private int _maxPoolSize = 20;

        private Queue<GameObject> _coinQueue;
        private Transform _poolRoot;

        protected override void Awake()
        {
            base.Awake();
            _coinQueue = new Queue<GameObject>(_maxPoolSize);
            _poolRoot = new GameObject("CoinPoolRoot").transform;
            _poolRoot.SetParent(transform);

            for (int i = 0; i < _maxPoolSize; i++)
            {
                GameObject coin = Instantiate(_coinPrefab, _poolRoot);
                coin.SetActive(false);
                _coinQueue.Enqueue(coin);
            }
        }

        public GameObject SpawnCoin(Vector3 position, Quaternion rotation)
        {
            GameObject coin = GetNextAvailableCoin();
            coin.transform.SetPositionAndRotation(position, rotation);
            coin.SetActive(true);
            return coin;
        }

        private GameObject GetNextAvailableCoin()
        {
            foreach (GameObject coin in _coinQueue)
            {
                if (!coin.activeInHierarchy)
                    return coin;
            }

            GameObject oldestCoin = _coinQueue.Dequeue();
            _coinQueue.Enqueue(oldestCoin);
            return oldestCoin;
        }

        public void ReturnCoin(GameObject coin)
        {
            coin.SetActive(false);
            coin.transform.SetParent(_poolRoot);
        }
    }
}