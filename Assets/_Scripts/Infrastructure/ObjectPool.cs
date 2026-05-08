using System.Collections.Generic;
using _Scripts.Interfaces;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Infrastructure
{
    public class ObjectPool<T> where T : MonoBehaviour, IPoolable
    {
        private readonly T _prefab;
        private readonly Transform _transform;
        private readonly Queue<T> _pool = new();
        public T Prefab => _prefab;
        public int TotalCreated { get; private set; }
        public int ReusedCount { get; private set; }
        public int AvailableCount => _pool.Count;

        public ObjectPool(T prefab, Transform transform, int prewarmCount)
        {
            _prefab = prefab;
            _transform = transform;

            for (var i = 0; i < prewarmCount; i++)
            {
                CreateNew();
            }
        }

        public T Get()
        {
            T item;
            if (_pool.Count > 0)
            {
                item = _pool.Dequeue();
                ReusedCount++;
            }
            else
            {
                item = CreateNew();
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
        
        private T CreateNew()
        {
            T item = Object.Instantiate(_prefab, _transform);
            item.gameObject.SetActive(false);
            _pool.Enqueue(item);
            TotalCreated++;
            return item;
        }
    }
}