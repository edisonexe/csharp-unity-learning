using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private float _spawnInterval = 4f;
    private Camera _cam;
    private static int _totalTargetsSpawned;
    
    private void Start()
    {
        _cam = Camera.main;
        _totalTargetsSpawned = 0;
        
        InvokeRepeating(nameof(SpawnTarget), 0f, _spawnInterval);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene("Menu");
    }

    private void SpawnTarget()
    {
        if (_targetPrefab == null || _cam == null) return;

        Vector3 pos = GetRandomWorldPosInView();
        Instantiate(_targetPrefab, pos, Quaternion.identity);

        _totalTargetsSpawned++;
        Debug.Log($"Targets spawned: {_totalTargetsSpawned}");
    }
    
    Vector3 GetRandomWorldPosInView()
    {
        float u = Random.Range(0.1f, 0.9f);
        float v = Random.Range(0.1f, 0.9f);
        float zFromCamera = 10f;

        return _cam.ViewportToWorldPoint(new Vector3(u, v, zFromCamera));
    }
}