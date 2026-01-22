using System;
using _Scripts.GameFSM;
using Player;
using Spawning;
using Events;
using Services;
using UnityEngine;
using UnityEngine.SceneManagement;

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
            if (_gameConfig == null || _enemySpawner == null || _itemSpawner == null || _playerView == null)
            {
                Debug.LogError("[Bootstrapper] GameConfig/Spawners/PlayerView - null. Отмена запуска.");
                enabled = false;
                return;
            }
            
            var enemySpawnInterval = _gameConfig.EnemySpawnInterval;
            var itemSpawnInterval = _gameConfig.ItemSpawnInterval;
        
            _gameStateService = new GameStateService(_gameConfig.TargetScore, _gameConfig.PlayerMaxHp);
            _enemySpawnService = new SpawnService(_enemySpawner, enemySpawnInterval, ENEMY_SPAWNED_T);
            _itemSpawnService = new SpawnService(_itemSpawner, itemSpawnInterval, ITEM_SPAWNED_T);
            _playerMoveService = new PlayerMoveService(_gameConfig.PlayerMoveSpeed);
        
            _gameStateService.Enable();
            _enemySpawnService.Enable();
            _itemSpawnService.Enable();
        
            EventBus.Subscribe(HandleGlobalEvent);
        }

        private void Start() => _gameStateService?.StartGame();

        private void Update()
        {
            _enemySpawnService.Update(Time.deltaTime);
            _itemSpawnService.Update(Time.deltaTime);
        }
        
        private void FixedUpdate()
        {
            if (_gameStateService.State != GameState.Playing) return;

            var delta = _playerMoveService.CalculateDelta(_playerView.MoveInput, Time.fixedDeltaTime);
            _playerView.ApplyMove(delta);
        }


        private void OnDestroy()
        {
            EventBus.Unsubscribe(HandleGlobalEvent);
            _enemySpawnService?.Disable();
            _itemSpawnService?.Disable();
            _gameStateService?.Disable();
        }
        
        private void HandleGlobalEvent(GameEventType type, int value)
        {
            if (type != GameEventType.RestartRequested) return;
            Time.timeScale = 1f;
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }   
}