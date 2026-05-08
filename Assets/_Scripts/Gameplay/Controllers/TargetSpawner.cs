using System.Collections;
using _Scripts.Gameplay.Services;
using UnityEngine;

namespace _Scripts.Gameplay.Controllers
{
    [AddComponentMenu("StressTest/Controllers/Target Spawner")]
    public class TargetSpawner : MonoBehaviour
    {
        [SerializeField] private Transform[] _spawnPoints;
        [SerializeField] private float _spawnInterval = 0.5f;
        
        private EntityFactory _factory;
        
        private Coroutine _spawnRoutine;
        private WaitForSeconds _wait;
        
        public void Init(EntityFactory factory)
        {
            if (factory==null)
            {
                Debug.LogError("[TargetSpawner] EntityFactory is null!");
                return;
            }

            if (_spawnPoints == null || _spawnPoints.Length == 0)
            {
                Debug.LogError("[TargetSpawner] Spawn Points array is empty or null!");
                return;
            }

            for (var i = 0; i < _spawnPoints.Length; i++)
            {
                if (!_spawnPoints[i])
                {
                    Debug.LogError($"[TargetSpawner] Spawn Point at index {i} is not assigned!");
                    return;
                }
            }

            _factory = factory;
            _wait = new WaitForSeconds(_spawnInterval);
        }

        public void StartSpawning()
        {
            StopSpawning();
            _spawnRoutine = StartCoroutine(SpawnRoutine());
        }

        public void StopSpawning()
        {
            if (_spawnRoutine != null)
            {
                StopCoroutine(_spawnRoutine);
                _spawnRoutine = null;
            }
        }

        private IEnumerator SpawnRoutine()
        {
            while (true)
            {
                SpawnRandomTarget();
                yield return _wait;
            }
        }
        
        private void SpawnRandomTarget()
        {
            int randomIndex = Random.Range(0, _spawnPoints.Length);
            Transform point = _spawnPoints[randomIndex];
            
            Vector2 randomCircle = Random.insideUnitCircle.normalized;
            Vector3 randomDirection = new Vector3(randomCircle.x, 0, randomCircle.y);

            _factory.CreateTarget(point.position, randomDirection);
        }
        
        private void OnDisable() => StopSpawning();
    }
}