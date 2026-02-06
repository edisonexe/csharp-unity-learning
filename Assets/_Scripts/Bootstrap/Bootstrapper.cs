using Characters.Player.Controllers;
using Characters.Player;
using Configs;
using Game;
using Services;
using Spawning;
using UI;
using UnityEngine;
using Weapon;

namespace Bootstrap
{
    public sealed class Bootstrapper : MonoBehaviour
    {
        [SerializeField] private Transform _playerTransform;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private PlayerConfig _playerCfg;
        [SerializeField] private GameConfig _gameCfg;
        [SerializeField] private EnemySpawner _enemySpawner;
        [SerializeField] private HudView _hudView;
    
        private PlayerEntity _playerEntity;
        private WeaponSet _weaponSet;
        private WaveSpawner _waveSpawner;
        private IntervalService _intervalService;
        private GameContext _run;
        private GameStateMachine _gsm;
        private GameRestartController _restartController;
    
        private const string ARENA_SCENE = "Arena";
        
        private void Awake()
        {
            _gsm = new GameStateMachine();
            _run = new GameContext();
            _restartController = new GameRestartController(_run, ARENA_SCENE);
            
            _playerEntity = new PlayerEntity(_playerCfg);
            _playerEntity.Died += OnPlayerDied;
        
            _playerController.Init(_playerEntity);
            
            _hudView.Init(_playerEntity, _run, _gsm);
            _hudView.RestartRequested += _restartController.Restart;
            
            _enemySpawner.Init(_playerEntity, _playerTransform, _run);
            _waveSpawner = new WaveSpawner(_enemySpawner, _gameCfg.EnemiesPerWave, _gameCfg.MaxWaves);
            _waveSpawner.AllWavesSpawned += () => _run.MarkAllWavesSpawned();
            _intervalService = new IntervalService(_waveSpawner.SpawnWave, _gameCfg.WaveInterval);
            _run.OnWin += OnPlayerWin;
        }

        private void Start()
        {
            _gsm.StartSession();
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

        private void OnPlayerWin()
        {
            _gsm.Win();
            _intervalService.Disable();
        }
    
        private void OnPlayerDied()
        {
            _gsm.Lose();
            _intervalService.Disable();
        }
    }
}