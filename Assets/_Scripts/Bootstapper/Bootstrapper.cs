using _Scripts.Enemy.Spawning;
using _Scripts.Player;
using Services;
using UnityEngine;

namespace Bootstapper
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private GameConfig _gameConfig;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private ItemSpawner _itemSpawner;
        [SerializeField] private PlayerView _playerView;
    
        private GameStateService _gameStateService;
        private SpawnService _enemySpawnService;
        private SpawnService _itemSpawnService;
        private PlayerMoveService _playerMoveService;
    
        private const GameEventType _enemySpawnedType = GameEventType.EnemySpawned;
        private const GameEventType _itemSpawnedType = GameEventType.ItemSpawned;
    
        private void Awake()
        {
            var enemySpawnInterval = _gameConfig.EnemySpawnInterval;
            var itemSpawnInterval = _gameConfig.ItemSpawnInterval;
        
            _gameStateService = new GameStateService(_gameConfig.TargetScore);
            _enemySpawnService = new SpawnService(_enemySpawner, enemySpawnInterval, _enemySpawnedType);
            _itemSpawnService = new SpawnService(_itemSpawner, itemSpawnInterval, _itemSpawnedType);
            _playerMoveService = new PlayerMoveService(_gameConfig.PlayerMoveSpeed);
        
            _gameStateService.Enable();
            _enemySpawnService.Enable();
            _itemSpawnService.Enable();
        
            _gameStateService.StartGame();
        }

        private void Update()
        {
            if (_gameStateService.State != GameState.Playing) return;
            _enemySpawnService.Update(Time.deltaTime);
            _itemSpawnService.Update(Time.deltaTime);
        
            var delta = _playerMoveService.CalculateDelta(_playerView.MoveInput, Time.deltaTime);
            _playerView.ApplyMove(delta);
        }

        private void OnDestroy()
        {
            _enemySpawnService.Disable();
            _itemSpawnService.Disable();
            _gameStateService.Disable();
        }
    }   
}