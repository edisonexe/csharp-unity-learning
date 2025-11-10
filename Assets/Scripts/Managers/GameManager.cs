using DefaultNamespace;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Managers
{
    public class GameManager : MonoBehaviour
    {
        [Header("Spawn Settings")]
        [SerializeField] private GameObject _targetPrefab;
        [SerializeField] private float _spawnInterval = 4f;
        [SerializeField] private float _minViewportBound = 0.1f;
        [SerializeField] private float _maxViewportBound = 0.9f;
        [SerializeField] private float _spawnDistanceFromCamera = 10f;

        [SerializeField] private GameEvents _events;
    
        private Camera _cam;
        private const string MENU_SCENE_NAME = "Menu";

        private void Start()
        {
            _cam = Camera.main;
            InvokeRepeating(nameof(SpawnTarget), 0f, _spawnInterval);
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                BackToMenu();
            if (Input.GetKeyDown(KeyCode.R))
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }

        public void BackToMenu() => SceneManager.LoadScene(MENU_SCENE_NAME);

        private void SpawnTarget()
        {
            if (_targetPrefab == null || _cam == null) return;

            Vector3 pos = GetRandomWorldPosInView();
            Instantiate(_targetPrefab, pos, Quaternion.identity);

            _events.RaiseTargetSpawned();
        }
    
        private Vector3 GetRandomWorldPosInView()
        {
            float randomX = Random.Range(_minViewportBound, _maxViewportBound);
            float randomY = Random.Range(_minViewportBound, _maxViewportBound);

            return _cam.ViewportToWorldPoint(new Vector3(randomX, randomY, _spawnDistanceFromCamera));
        }
    }
}