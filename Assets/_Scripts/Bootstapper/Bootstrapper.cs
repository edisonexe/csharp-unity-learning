using _Scripts.Services;
using _Scripts.Spawning;
using UnityEngine;

public sealed class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private EnemySpawner _enemySpawner;

    private GameStateService _gameStateService;
    private SpawnService _spawnService;

    private void Awake()
    {
        _gameStateService = new GameStateService(_gameConfig.TargetScore);
        _spawnService = new SpawnService(_enemySpawner, _gameConfig.EnemySpawnInterval);

        _gameStateService.Enable();
        _spawnService.Enable();

        _gameStateService.StartGame();
    }

    private void Update()
    {
        if (_gameStateService.State != GameState.Playing) return;
        _spawnService.Tick(Time.deltaTime);
    }

    private void OnDestroy()
    {
        _spawnService.Disable();
        _gameStateService.Disable();
    }
}