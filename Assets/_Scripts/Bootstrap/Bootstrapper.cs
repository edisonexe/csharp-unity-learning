using Characters.Player;
using Services;
using Spawning;
using Configs;
using Game;
using UnityEngine;
using UI;

public sealed class Bootstrapper : MonoBehaviour
{
    [SerializeField] private Transform _playerTransform;
    [SerializeField] private PlayerController _playerController;
    [SerializeField] private PlayerConfig _playerCfg;
    [SerializeField] private GameConfig _gameCfg;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private HudView _hudView;
    
    private PlayerEntity _playerEntity;
    private WaveSpawner _waveSpawner;
    private IntervalService _intervalService;
    private GameContext _run;
    
    private void Awake()
    {
        _playerEntity = new PlayerEntity(_playerCfg.MaxHp, _playerCfg.Damage, _playerCfg.MoveSpeed);
        _playerController.Init(_playerEntity);
        _playerEntity.Died += OnPlayerDied;

        _run = new GameContext();
        _hudView.Init(_playerEntity, _run);
        _enemySpawner.Init(_playerEntity, _playerTransform, _run);
        _waveSpawner = new WaveSpawner(_enemySpawner, _gameCfg.EnemiesPerWave, _gameCfg.MaxWaves);
        _intervalService = new IntervalService(_waveSpawner.SpawnWave, _gameCfg.WaveInterval);
    }

    private void Start()
    {
        _intervalService.Enable();
    }

    private void Update()
    {
        _intervalService.Update(Time.deltaTime);
    }

    private void OnDestroy()
    {
        if (_playerEntity != null)
            _playerEntity.Died -= OnPlayerDied;

        _intervalService.Disable();
    }
    
    private void OnPlayerDied() => _intervalService.Disable();
}