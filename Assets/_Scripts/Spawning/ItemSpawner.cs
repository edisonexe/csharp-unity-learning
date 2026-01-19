using System.Collections.Generic;
using UnityEngine;

namespace Spawning
{
    public sealed class ItemSpawner : MonoBehaviour, ISpawner
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private List<Transform> _spawnPoints;

        public void SpawnOne()
        {
            if (_spawnPoints == null || _spawnPoints.Count == 0)
            {
                Debug.LogWarning("[ItemSpawner]: Нет точек спавна");
                return;
            }

            int index = Random.Range(0, _spawnPoints.Count);
            Transform point = _spawnPoints[index];

            Instantiate(_itemPrefab, point.position + Vector3.up, point.rotation);
        }
    }
}