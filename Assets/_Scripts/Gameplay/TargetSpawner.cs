using UnityEngine;

namespace StressTest.Gameplay
{
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnInterval = 0.5f;
        
        private PoolSystem _poolSystem;
        private float _spawnTimer;
        private bool _isInitialized;

        public void Init(PoolSystem poolSystem)
        {
            if (!poolSystem)
            {
                Debug.LogError("[TargetSpawner] PoolSystem is null!");
                return;
            }

            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                Debug.LogError("[TargetSpawner] Spawn Points array is empty or null!");
                return;
            }

            for (int i = 0; i < _spawnPoints.Length; i++)
            {
                if (!_spawnPoints[i])
                {
                    Debug.LogError($"[TargetSpawner] Spawn Point at index {i} is not assigned!");
                    return;
                }
            }

            _poolSystem = poolSystem;
            _isInitialized = true;
        }

        private void Update()
        {
            if (!_isInitialized) return;

            _spawnTimer += Time.deltaTime;
            if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0;
                SpawnRandomTarget();
            }
        }

        private void SpawnRandomTarget()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            Transform point = _spawnPoints[randomIndex];
            
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            Vector3 randomDirection = new Vector3(randomCircle.x, 0, randomCircle.y);

            _poolSystem.SpawnTarget(point.position, randomDirection);
        }
    }
}