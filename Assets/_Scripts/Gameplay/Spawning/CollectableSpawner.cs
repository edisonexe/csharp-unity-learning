using System.Collections;
using System.Collections.Generic;
using _Scripts.Configs;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.Spawning
{
    public sealed class CollectableSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private SpawnerConfig _cfg;
        [SerializeField] private Transform[] _spawnPoints;

        private int _totalSpawned;
        
        private ICollectableFactory _factory;
        private ILoggerService _logger;

        private Coroutine _routine;
        private bool _isInitialized;

        private readonly List<GameObject> _spawned = new();
        
        public void Init(ICollectableFactory factory, ILoggerService logger)
        {
            _logger = logger;

            if (_isInitialized)
            {
                _logger.Warn("CollectableSpawner is already initialized");
                return;
            }
            if (!_cfg)
            {
                _logger.Error("CollectableSpawner: cfg is null");
                return;
            }

            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                _logger?.Error("CollectableSpawner: spawn points not set!");
                return;
            }
            _factory = factory;
            if (_factory == null)
            {
                _logger?.Error("CollectableSpawner: factory is null");
                return;
            }
            
            
            _isInitialized = true;
        }

        public void StartSpawning()
        {
            if (!_isInitialized)
            {
                _logger.Error("CollectableSpawner is not initialized");
                return;
            }

            if (_routine != null) return;
            
            _routine = StartCoroutine(SpawnLoop());
            _logger?.Log("Collectable spawner started");
        }

        public void StopSpawning()
        {
            if (_routine == null) return;

            StopCoroutine(_routine);
            _routine = null;
            _logger?.Log("Collectable spawner stopped");
        }

        public void ClearSpawned()
        {
            _totalSpawned = 0;
            for (var i = 0; i < _spawned.Count; i++)
                if (_spawned[i]) Destroy(_spawned[i]);

            _spawned.Clear();
        }
        
        private IEnumerator SpawnLoop()
        {
            while (_totalSpawned < _cfg.MaxTotal)
            {
                yield return new WaitForSeconds(_cfg.Interval);
                
                if (_spawnPoints == null || _spawnPoints.Length == 0)
                    continue;
                
                var point = _spawnPoints[Random.Range(0, _spawnPoints.Length)];
                if (!point) continue;

                var spawned = _factory.Spawn(point);
                if (spawned)
                {
                    _spawned.Add(spawned.gameObject); 
                    _totalSpawned++;
                }
            }
            _routine = null;
        }
        
    }
}