using Input;
using Pooling;
using UnityEngine;
using UnityEngine.AI;
using Weapon;

namespace Characters.Player.Controllers
{
    [RequireComponent (typeof(NavMeshAgent), typeof(PlayerInputReader))]

    public sealed class PlayerController : MonoBehaviour
    {
        [Header("Move")]
        [SerializeField] private LayerMask _groundMask = ~0;
        [SerializeField][Min(1f)] private float _clickRayRange = 500f;

        [Header("Camera")]
        [SerializeField] private UnityEngine.Camera _camera;
        [SerializeField] private Transform _cameraRigRoot;
        [SerializeField][Min(0.1f)] private float _yawSpeed = 0.2f;

        [Header("RMB: click/hold")]
        [SerializeField][Min(0.15f)] private float _holdThreshold = 0.15f;
        [SerializeField][Min(1)] private float _dragThresholdPixels = 8f;

        [Header("Shooting")]
        [SerializeField] private Transform _shootingPoint;
        [SerializeField] private LayerMask _shootMask = ~0;
        [SerializeField] private LayerMask _aimMask = ~0;
        [SerializeField] private ShootTracerView _tracerPrefab;

        [Header("PoolHub")]
        [SerializeField] private PoolHub _poolHub;
        
        private NavMeshAgent _agent;

        private PlayerEntity _playerEntity;

        private WeaponSet _weaponSet;
        private WeaponModel _weapon;

        private PlayerInputReader _input;
        private PlayerMovementController _movement;
        private PlayerAimController _aimController;
        private PlayerShootController _shootController;
        private PlayerWeaponController _weaponController;

        public void Init(PlayerEntity playerEntity)
        {
            _playerEntity = playerEntity;
            _playerEntity.Died += OnDied;

            _weaponSet = _playerEntity.Weapons;
            _weaponSet.Changed += OnWeaponChanged;
            OnWeaponChanged(_weaponSet.Current);
        }

        private void Awake()
        {
            _agent = GetComponent<NavMeshAgent>();
            _agent.updateRotation = false;

            if (!_input) _input = GetComponent<PlayerInputReader>();

            if (!_camera) _camera = UnityEngine.Camera.main;
            if (_cameraRigRoot == null && _camera != null)
                _cameraRigRoot = _camera.transform.parent != null ? _camera.transform.parent : _camera.transform;
        }

        private void Start()
        {
            _movement = new PlayerMovementController(_agent, _camera, _cameraRigRoot, _groundMask, _clickRayRange,
                _yawSpeed, _holdThreshold, _dragThresholdPixels);
            _aimController = new PlayerAimController(_camera, _aimMask);
            _shootController = new PlayerShootController(_shootingPoint, _shootMask, _tracerPrefab, _poolHub);
            _weaponController = new PlayerWeaponController();

            _movement.SetSpeed(_playerEntity.MoveSpeed);
        }

        private void Update()
        {
            if (_playerEntity == null || !_playerEntity.IsAlive)
                return;

            _movement.Update(_input);
            _weaponController.Update(_input, _weaponSet, _weapon);
            _aimController.Update(_input, transform, _weapon);
            _shootController.Update(_input, _weapon, _aimController, transform);
        }

        private void OnDestroy()
        {
            if (_playerEntity != null)
                _playerEntity.Died -= OnDied;

            if (_weaponSet != null)
                _weaponSet.Changed -= OnWeaponChanged;
        }

        private void OnWeaponChanged(WeaponModel newWeapon) => _weapon = newWeapon;

        private void OnDied()
        {
            _movement.Stop();
            _agent.enabled = false;
        }
    }
}