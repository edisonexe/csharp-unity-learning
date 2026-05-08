using System.Collections.Generic;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Infrastructure
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T _prefab;
        private readonly Transform _transform;
        private readonly Queue<T> _pool = new();

        private int _totalCreated;
        private int _reusedCount;

        public T Prefab => _prefab;
        public int TotalCreated => _totalCreated;
        public int ReusedCount => _reusedCount;
        public int AvailableCount => _pool.Count;

        public ObjectPool(T prefab, Transform transform, int prewarmCount)
        {
            _prefab = prefab;
            _transform = transform;

            for (var i = 0; i < prewarmCount; i++)
            {
                _pool.Enqueue(CreateInternal());
            }
        }

        public T Get()
        {
            T item;
            if (_pool.Count > 0)
            {
                item = _pool.Dequeue();
                _reusedCount++;
            }
            else
            {
                item = CreateInternal();
            }

            item.gameObject.SetActive(true);
            item.OnSpawn();
            return item;
        }

        public void Return(T item)
        {
            item.OnDespawn();
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
        }

        private T CreateInternal()
        {
            T item = Object.Instantiate(_prefab, _transform);
            item.gameObject.SetActive(false);
            _totalCreated++;
            return item;
        }
    }
}