using System.Collections.Generic;
using UnityEngine;

namespace Spawning
{
    public sealed class EnemySpawner : MonoBehaviour, IEnemyFactory
    {
        [SerializeField] private GameObject enemyPrefab;
        [SerializeField] private List<Transform> spawnPoints;

        public void SpawnOne()
        {
            if (spawnPoints == null || spawnPoints.Count == 0)
            {
                Debug.LogWarning("[EnemySpawner]: Нет точек спавна");
                return;
            }

            Transform point = GetRandomSpawnPoint();
            Instantiate(enemyPrefab, point.position, point.rotation);
        }

        private Transform GetRandomSpawnPoint()
        {
            var index = Random.Range(0, spawnPoints.Count);
            return spawnPoints[index];
        }
    }
}
