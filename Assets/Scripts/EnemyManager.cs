using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    [SerializeField] private GameObject _enemyPrefab;
    
    private void OnEnable() => EventManager.OnEnemySpotted += OnEnemySpotted;

    private void OnDisable() => EventManager.OnEnemySpotted += OnEnemySpotted;

    private void OnEnemySpotted(float x, float z)
    {
        if (!_enemyPrefab)
        {
            Debug.LogWarning("Префаб врага не задан.");
            return;
        }

        Vector3 spawnPos = new Vector3(x, 1, z);

        Instantiate(_enemyPrefab, spawnPos, Quaternion.identity);

        Debug.Log($"[EnemyManager] Спавн врага по координатам ({x}, 1, {z})");
    }
}