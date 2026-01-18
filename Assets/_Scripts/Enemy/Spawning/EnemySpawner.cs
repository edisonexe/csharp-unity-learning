using System.Collections.Generic;
using Enemy;
using UnityEngine;

namespace _Scripts.Enemy.Spawning
{
    public sealed class EnemySpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private Transform _player;
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private List<Transform> _spawnPoints;
        [SerializeField] private List<Transform> _patrolPoints;
        
        
        public void SpawnOne()
        {
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                Debug.LogWarning("[EnemySpawner]: Нет точек спавна");
                return;
            }

            Transform point = GetRandomSpawnPoint();
            var go = Instantiate(_enemyPrefab, point.position + Vector3.up, point.rotation);
            
            var enemy = go.GetComponent<EnemyView>();
            if (enemy != null)
            {
                enemy.SetPlayer(_player);
                enemy.SetPatrolPoints(_patrolPoints);
            }
        }

        private Transform GetRandomSpawnPoint()
        {
            var index = Random.Range(0, _spawnPoints.Count);
            return _spawnPoints[index];
        }
    }
}
