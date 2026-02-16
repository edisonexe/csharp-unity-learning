using _Scripts.Camera;
using _Scripts.Configs;
using _Scripts.Interfaces;
using _Scripts.Player;
using _Scripts.Services.Input;
using _Scripts.Services.Logging;
using _Scripts.Services.Movement;
using UnityEngine;

namespace _Scripts.Installer
{
    public sealed class GameInstaller : MonoBehaviour
    {
        [Header("Setup")]
        [SerializeField] private PlayerConfig _playerCfg;
        [SerializeField] private InputMode _inputMode;
    
        [Header("Scene Refs")]
        [SerializeField] private PlayerController _player;
        [SerializeField] private CameraRigController _cameraRig;
        [SerializeField] private Rigidbody _playerRb;
        [SerializeField] private Transform _aiTarget;
        [SerializeField] private DamageZone[] _damageZones;
        [SerializeField] private Collectable[] _collectables;

        private IInputService _input;
        private ILookInputService _lookInput;
        private ICameraDirectionProvider _cameraDir;
        private IMovementService _movement;
        private IHealth _playerHealth;
        private ILoggerService _logger;
        private HealthLogger _healthLogger;

        private void Awake()
        {
            Compose();
            Inject();

            _logger.Log("Game started");
        }

        private void Compose()
        {
            _logger = new UnityLoggerService();

            _input = _inputMode switch
            {
                InputMode.Keyboard => new KeyboardInputService(),
                InputMode.AI => new AIInputService(_player.transform, _aiTarget),
                InputMode.Fake => new FakeInputService(Vector2.right),
                _ => new KeyboardInputService()
            };
            _lookInput = _input as ILookInputService;
            _cameraDir = new CameraDirectionProvider(_cameraRig.transform);
            
            _logger.Log($"Input mode: {_inputMode}");

            _movement = new RigidbodyMovementService(_playerRb, _playerCfg.MoveSpeed);
            
            _playerHealth = new HealthModel(_playerCfg.MaxHp, _logger);
            _healthLogger = new HealthLogger(_playerHealth, _logger);
        }

        private void Inject()
        {
            _cameraRig.Init(_lookInput, _player.transform);
            _player.Init(_input, _movement, _cameraDir);

            foreach (var dz in _damageZones)
                dz.Init(_playerHealth);

            foreach (var c in _collectables)
                c.Init(_logger);
        }
    }
}