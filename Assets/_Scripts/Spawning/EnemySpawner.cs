using System.Collections.Generic;
using Characters.Enemy;
using Characters.Player;
using Game;
using Pooling;
using UnityEngine;
using UnityEngine.AI;

namespace Spawning
{
    public sealed class EnemySpawner : MonoBehaviour, ISpawner
    {
        [Header("Player Transform")]
        private Transform _playerTransform;
    
        [Header("Enemy Prefabs")]
        [SerializeField] private List<EnemyController> _enemiesList;
        
        [Header("Spawn Points")]
        [SerializeField] private List<Transform> _spawnPoints;
        
        [SerializeField] private PoolHub _poolHub;
        private readonly Dictionary<EnemyController, ObjectPool<EnemyController>> _enemyPools = new();
        
        private readonly List<EnemyController> _alive = new();
        
        private PlayerEntity _playerEntity;
        private GameContext _run;
        
        public void Init(PlayerEntity playerEntity, Transform playerTransform, GameContext run)
        {
            if (_playerEntity != null)
                _playerEntity.Died -= OnPlayerDied;
            
            _playerEntity = playerEntity;
            _playerTransform = playerTransform;
            _run = run;
            
            if (_playerEntity != null)
                _playerEntity.Died += OnPlayerDied;
        }
        
        public void SpawnOne()
        {
            var prefab = _enemiesList[Random.Range(0, _enemiesList.Count)];
            var point = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            Vector3 pos = point.position;

            bool found = NavMesh.SamplePosition(pos, out var hit, 2f, NavMesh.AllAreas);

            if (!found) return;

            pos = hit.position;

            var pool = GetEnemyPool(prefab);
            var enemy = pool.Get(pos, Quaternion.identity);
            
            
            _run.EnemySpawned();
            enemy.BindPool(pool);
            _alive.Add(enemy);
            
            enemy.SetOnDied(OnEnemyDied);
            
            enemy.ConstructEnemy(_playerEntity, _playerTransform);
        }
        
        private ObjectPool<EnemyController> GetEnemyPool(EnemyController prefab)
        {
            if (_enemyPools.TryGetValue(prefab, out var pool))
                return pool;

            pool = _poolHub.GetPool(prefab);
            _enemyPools[prefab] = pool;
            return pool;
        }

        private void OnEnemyDied(EnemyController enemy)
        {
            _alive.Remove(enemy);
            _run.EnemyKilled();
        }
        
        private void OnPlayerDied()
        {
            for (var i = _alive.Count - 1; i >= 0; i--)
            {
                var e = _alive[i];
                if (e) e.ForceDespawn();
            }
            _alive.Clear();
        }
        
        private void OnDestroy()
        {
            if (_playerEntity != null)
                _playerEntity.Died -= OnPlayerDied;
        }
    }
}