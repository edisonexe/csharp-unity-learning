using Player;
using Spawning;
using Events;
using Services;
using UnityEngine;

namespace Bootstrap
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
    
        private const GameEventType ENEMY_SPAWNED_T = GameEventType.EnemySpawned;
        private const GameEventType ITEM_SPAWNED_T = GameEventType.ItemSpawned;
    
        private void Awake()
        {
            var enemySpawnInterval = _gameConfig.EnemySpawnInterval;
            var itemSpawnInterval = _gameConfig.ItemSpawnInterval;
        
            _gameStateService = new GameStateService(_gameConfig.TargetScore);
            _enemySpawnService = new SpawnService(_enemySpawner, enemySpawnInterval, ENEMY_SPAWNED_T);
            _itemSpawnService = new SpawnService(_itemSpawner, itemSpawnInterval, ITEM_SPAWNED_T);
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