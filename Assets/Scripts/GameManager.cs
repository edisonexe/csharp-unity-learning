using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject _targetPrefab;
    [SerializeField] private float _spawnInterval = 4f;
    [SerializeField] private float _minViewportBound = 0.1f;
    [SerializeField] private float _maxViewportBound = 0.9f;
    [SerializeField] private float _spawnDistanceFromCamera = 10f;
    
    [Header("UI Settings")]
    [SerializeField] private Text _targetsSpawnedText;
    
    private Camera _cam;
    private int _totalTargetsSpawned = 0;
    private const string MENU_SCENE_NAME = "Menu";
    
    private void Start()
    {
        _cam = Camera.main;
        InvokeRepeating(nameof(SpawnTarget), 0f, _spawnInterval);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            SceneManager.LoadScene(MENU_SCENE_NAME);
        if (Input.GetKeyDown(KeyCode.R))
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void SpawnTarget()
    {
        if (_targetPrefab == null || _cam == null) return;

        Vector3 pos = GetRandomWorldPosInView();
        Instantiate(_targetPrefab, pos, Quaternion.identity);

        _totalTargetsSpawned++;
        UpdateUIText();
        Debug.Log($"Targets spawned: {_totalTargetsSpawned}");
    }
    
    private Vector3 GetRandomWorldPosInView()
    {
        float randomX = Random.Range(_minViewportBound, _maxViewportBound);
        float randomY = Random.Range(_minViewportBound, _maxViewportBound);

        return _cam.ViewportToWorldPoint(new Vector3(randomX, randomY, _spawnDistanceFromCamera));
    }
    
    private void UpdateUIText()
    {
        if (_targetsSpawnedText != null)
            _targetsSpawnedText.text = $"Targets spawned: {_totalTargetsSpawned}";
    }
}