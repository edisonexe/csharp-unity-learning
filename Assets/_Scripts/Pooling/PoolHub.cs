using System;
using System.Collections.Generic;
using UnityEngine;

namespace Pooling
{
    public sealed class PoolHub : MonoBehaviour
    {
        [Serializable]
        public sealed class PoolEntry
        {
            [SerializeField] private MonoBehaviour _prefab;
            [SerializeField] private int _preloadCount = 8;
            public MonoBehaviour Prefab => _prefab;
            public int PreloadCount => _preloadCount;
        }
        
        [SerializeField] private List<PoolEntry> _pools = new ();
        private readonly Dictionary<MonoBehaviour, object> _map = new();

        private void Awake()
        {
            foreach (var entry in _pools)
            {
                if (!entry.Prefab) continue;
                var prefab = entry.Prefab;

                var root = new GameObject($"Pool_{prefab.name}").transform;
                root.SetParent(transform, false);
                
                var type = prefab.GetType();
                var poolType = typeof(ObjectPool<>).MakeGenericType(type);
                
                var pool = Activator.CreateInstance(poolType, prefab, entry.PreloadCount, root);
                _map[prefab] = pool;
            }
        }
        
        public ObjectPool<T> GetPool<T>(T prefab) where T : MonoBehaviour
        {
            if (!_map.TryGetValue(prefab, out var poolObj))
                throw new KeyNotFoundException($"Pool for prefab '{prefab.name}' not found in PoolHub.");

            return (ObjectPool<T>)poolObj;
        }
    }
}