using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public sealed class ObjectPool<T> where T : Component
    {
        private T _prefab;
        private Transform _root;
        private Queue<T> _pool = new Queue<T>();

        public ObjectPool(T prefab, int count, Transform root = null)
        {
            _prefab = prefab;
            _root = root;

            for (var i = 0; i < count; i++)
            {
                _pool.Enqueue(CreateNew());
            }
        }
        
        private T CreateNew()
        {
            var obj = Object.Instantiate(_prefab, _root);
            obj.gameObject.SetActive(false);
            return obj;
        }

        public T Get(Vector3 pos, Quaternion rotation)
        {
            T obj = _pool.Count > 0 ? _pool.Dequeue() : CreateNew();
            var t = obj.transform;
            t.SetPositionAndRotation(pos, rotation);
            
            obj.gameObject.SetActive(true);
            
            if(obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnSpawned();
            
            return obj;
        }

        public void Return(T obj)
        {
            if (!obj) return;

            if (obj.TryGetComponent<IPoolable>(out var poolable))
                poolable.OnDespawned();

            obj.gameObject.SetActive(false);
            obj.transform.SetParent(_root, false);

            _pool.Enqueue(obj);

        }
    }
}