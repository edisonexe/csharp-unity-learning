using System.Collections.Generic;
using _Scripts.Camera;
using _Scripts.Configs;
using _Scripts.Configs.Modifiers;
using _Scripts.GameFlow;
using _Scripts.Gameplay.Factories;
using _Scripts.Gameplay.GameModifiers;
using _Scripts.Gameplay.Spawning;
using _Scripts.Gameplay.World;
using _Scripts.GSM;
using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Interfaces.Spawn;
using _Scripts.Models;
using _Scripts.Player;
using _Scripts.Services.Input;
using _Scripts.Services.Logging;
using _Scripts.Services.Movement;
using _Scripts.UI;
using UnityEngine;
using IGameResult = _Scripts.Interfaces.IGameResult;

namespace _Scripts.Bootstrap.GameScene
{
    public sealed class SceneBootstrapper : MonoBehaviour
    {
        [Header("Configs")]
        [SerializeField] private PlayerConfig _playerCfg;
        [SerializeField] private GameRulesConfig _gameCfg;
        [SerializeField] private GameModifiersConfig _modifiersConfig;

        [Header("Scene Refs")]
        [SerializeField] private PlayerController _player;
        [SerializeField] private Rigidbody _playerRb;
        [SerializeField] private Transform _playerStart;
        [SerializeField] private CameraRigController _cameraRig;
        [SerializeField] private Transform _aiTarget;

        [Header("Damage Zones")]
        [SerializeField] private DamageZone[] _damageZones;

        [Header("Spawning")]
        [SerializeField] private Collectable _prefab;
        [SerializeField] private CollectableSpawner _collectableSpawner;

        [Header("UI")]
        [SerializeField] private MainMenuView _mainMenuView;
        [SerializeField] private PauseView _pauseView;
        [SerializeField] private GameplayHud _gameplayHud;
        [SerializeField] private GameOverView _gameOverView;

        [Header("Controllers")]
        [SerializeField] private GameController _gameController;
        
        private IInputService _input;
        private bool _installed;

        public void Install(ILoggerService logger, ITimeService time, IPauseInputService pauseInput, 
            IInputModeProvider inputModeProvider, GameInputActions inputActions, GameStateMachine gsm)
        {
            if (_installed)
                return;

            _installed = true;
            
            IScore score = new ScoreModel();
            IGameResult gameResult = new GameResultModel();
            IHealth health = new HealthModel(_playerCfg.MaxHp);
            _ = new HealthLogger(health, logger);
            
            ICameraDirectionProvider cameraDir = new CameraDirectionProvider(_cameraRig.transform);

            IMovementService movement = new RigidbodyMovementService(_playerRb, _playerCfg.MoveSpeed);

            ICollectableFactory factory = new CollectableFactory(_prefab, score, logger);

            _collectableSpawner.Init(factory, logger);

            switch (inputModeProvider.CurrentMode)
            {
                case InputMode.AI:
                    if (!_aiTarget) logger.Error("AI Target not set");
                    _input = new AIInputService(_player.transform, _aiTarget);
                    break;

                case InputMode.Fake:
                    _input = new FakeInputService(Vector2.right);
                    break;

                case InputMode.Keyboard:
                default:
                    _input = new KeyboardInputService(inputActions);
                    break;
            }
            IInputToggle inputToggle = _input as IInputToggle;
            if (inputToggle == null) logger.Warn("Selected input mode does not implement IInputToggle");

            ILookInputService lookInput = _input as ILookInputService;
            
            var modifierContext = new ModifierContext(logger, health, inputToggle, _collectableSpawner as ISpawnRate);
            
            var modifiers = new List<IGameModifier>();
            var modifierNames = new List<string>();
            if (_modifiersConfig != null)
            {
                foreach (var def in _modifiersConfig.Modifiers)
                {
                    if (!def) continue;
                    modifiers.Add(def.Create(modifierContext));
                    modifierNames.Add(def.DisplayName);
                }
            }
            var modifierRunner = new ModifierRunner(modifiers);
            
            _cameraRig.Init(lookInput, _player.transform);
            _player.Init(_input, movement, cameraDir, logger);

            foreach (var dz in _damageZones)
                dz.Init(health);
            
            IGameSession session = new GameSession(health, score, gameResult, _collectableSpawner, _playerRb, 
                _playerStart);
            
            _mainMenuView.Init(logger);
            _pauseView.Init(logger);
            _gameplayHud.Init(logger);
            _gameOverView.Init(logger);

            gsm.Clear();

            gsm.Register(new MainMenuState(logger, _mainMenuView, gsm, inputToggle));
            gsm.Register(new GameplayState(logger, _gameplayHud, gsm, _collectableSpawner, inputModeProvider, 
                health, score, _gameCfg, gameResult, inputToggle, modifierRunner, modifierNames));
            gsm.Register(new PauseState(logger, _pauseView, gsm, time, inputToggle, session));
            gsm.Register(new GameOverState(logger, _gameOverView, gsm, gameResult, inputToggle, session));

            _gameController.Init(gsm, pauseInput, logger, modifierRunner);

            gsm.ChangeState<MainMenuState>();
        }
    }
}
