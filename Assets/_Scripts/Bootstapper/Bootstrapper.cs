using _Scripts.Enemy.Spawning;
using _Scripts.Player;
using Services;
using UnityEngine;

public sealed class Bootstrapper : MonoBehaviour
{
    [SerializeField] private GameConfig _gameConfig;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private PlayerView _playerView;
    
    private GameStateService _gameStateService;
    private SpawnService _spawnService;
    private PlayerMoveService _playerMoveService;
    
    private void Awake()
    {
        _gameStateService = new GameStateService(_gameConfig.TargetScore);
        _spawnService = new SpawnService(_enemySpawner, _gameConfig.EnemySpawnInterval);
        _playerMoveService = new PlayerMoveService(_gameConfig.PlayerMoveSpeed);
        
        _gameStateService.Enable();
        _spawnService.Enable();
        
        _gameStateService.StartGame();
    }

    private void Update()
    {
        if (_gameStateService.State != GameState.Playing) return;
        _spawnService.Tick(Time.deltaTime);
        
        var delta = _playerMoveService.CalculateDelta(_playerView.MoveInput, Time.deltaTime);
        _playerView.ApplyMove(delta);
    }

    private void OnDestroy()
    {
        _spawnService.Disable();
        _gameStateService.Disable();
    }
}