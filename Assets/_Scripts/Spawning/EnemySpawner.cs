using System.Collections.Generic;
using Characters.Enemy;
using Characters.Player;
using Game;
using Interfaces;
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
        
        private PlayerEntity _playerEntity;
        private GameContext _run;
        
        public void Init(PlayerEntity playerEntity, Transform playerTransform, GameContext run)
        {
            _playerEntity = playerEntity;
            _playerTransform = playerTransform;
            _run = run;
        }
        
        public void SpawnOne()
        {
            var prefab = _enemiesList[Random.Range(0, _enemiesList.Count)];
            var point = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

            Vector3 pos = point.position;

            bool found = NavMesh.SamplePosition(pos, out var hit, 5f, NavMesh.AllAreas);

            if (!found)
            {
                Debug.LogWarning("Spawn skipped: no NavMesh near spawn point");
                return;
            }

            pos = hit.position;

            var enemy = Instantiate(prefab, pos, Quaternion.identity);
            enemy.ConstructEnemy(_playerEntity, _playerTransform);
            enemy.Died += OnEnemyDied;
        }
        
        private void OnEnemyDied(EnemyController enemy)
        {
            enemy.Died -= OnEnemyDied;
            _run.AddKill();
        }
    }
}