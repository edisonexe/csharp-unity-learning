using System;
using System.Collections.Generic;
using _Project._Scripts.Gameplay.Camera;
using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Gameplay.Inventory;
using _Project._Scripts.Gameplay.Movement;
using _Project._Scripts.Gameplay.Player;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Views;
using _Project._Scripts.UI.Controllers;
using _Project._Scripts.UI.Views;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CharacterControllerMotor))]
    [RequireComponent(typeof(MouseLookController))]
    [RequireComponent(typeof(GamePlayerView))]
    [RequireComponent(typeof(RemotePlayerInterpolator))]
    [RequireComponent(typeof(Health))]
    [RequireComponent(typeof(Weapon))]
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(PlayerItemCollector))]
    [RequireComponent(typeof(PlayerConsumableService))]
    public sealed class GamePlayer : NetworkBehaviour
    {
        private Health _health;
        private Weapon _weapon;

        private CharacterController _characterController;
        private CharacterControllerMotor _motor;
        private MouseLookController _lookController;
        private GamePlayerView _playerView;
        private RemotePlayerInterpolator _remoteInterpolator;

        private GamePlayerPresentation _presentation;
        private LocalGamePlayerController _localController;
        
        private PlayerInventory _inventory;
        private PlayerItemCollector _itemCollector;
        private PlayerConsumableService _consumableService;
        private PlayerHudPresenter _hudPresenter;

        private IPlayerHudView _hudView;
        private bool _localDependenciesInjected;

        private static readonly List<GamePlayer> _players = new();

        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;

        [SyncVar(hook = nameof(OnColorChanged))]
        private Color _color = Color.white;

        [SyncVar] private Vector3 _serverPosition;
        [SyncVar] private float _serverYaw;
        [SyncVar] private float _serverPitch;
        [SyncVar(hook = nameof(OnAliveChanged))] private bool _isAlive = true;

        public Health Health => _health;
        public Weapon Weapon => _weapon;
        public bool IsAlive => _isAlive;
        public static int PlayerCount => _players.Count;

        public static event Action PlayerCountChanged;
        public static event Action<GamePlayer> LocalPlayerSpawned;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _motor = GetComponent<CharacterControllerMotor>();
            _lookController = GetComponent<MouseLookController>();
            _playerView = GetComponent<GamePlayerView>();
            _remoteInterpolator = GetComponent<RemotePlayerInterpolator>();
            _health = GetComponent<Health>();
            _weapon = GetComponent<Weapon>();
            
            _inventory = GetComponent<PlayerInventory>();
            _itemCollector = GetComponent<PlayerItemCollector>();
            _consumableService = GetComponent<PlayerConsumableService>();

            if (!_characterController || !_motor || !_lookController || !_playerView || !_remoteInterpolator || !_health || !_weapon)
            {
                Debug.LogError("[GamePlayer] Required components are missing.", this);
                enabled = false;
                return;
            }

            _motor.Construct(_characterController);
            _presentation = new GamePlayerPresentation(_playerView);
        }
        
        public void ConstructLocal(IPlayerHudView hudView)
        {
            if (hudView == null)
            {
                Debug.LogError("[GamePlayer] Local HUD view is not assigned.", this);
                return;
            }

            if (_localDependenciesInjected)
                return;

            _hudView = hudView;
            _localDependenciesInjected = true;
            
            _hudPresenter = new PlayerHudPresenter(_hudView, _health, _inventory);
            
            if (_isAlive)
                _hudView.Show();
            else
                _hudView.Hide();
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            gameObject.name = $"GamePlayer_{netId}";
        }
        
        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!_players.Contains(this))
                _players.Add(this);

            gameObject.name = $"GamePlayer_{netId}";
            
            PlayerCountChanged?.Invoke();
            
            _presentation?.Apply(_nickname, _color, isLocalPlayer);
            _presentation?.SetAlive(_isAlive);
        }

        public override void OnStopClient()
        {
            _players.Remove(this);
            PlayerCountChanged?.Invoke();

            base.OnStopClient();
        }

        public override void OnStartLocalPlayer()
        {
            base.OnStartLocalPlayer();

            if (!enabled)
                return;

            _localController ??= new LocalGamePlayerController(_playerView, _lookController, _remoteInterpolator,
                _weapon, _itemCollector, _consumableService);

            _localController.Initialize(CmdSendInput);

            _playerView?.SetLocalState(true);
            _playerView?.SetAliveState(_isAlive);

            LocalPlayerSpawned?.Invoke(this);
        }

        private void Update()
        {
            if (!enabled)
                return;

            if (isLocalPlayer)
            {
                UpdateLocalPlayer();
                return;
            }

            UpdateRemotePlayer();
        }

        [Server]
        public void Initialize(string nickname, Color color)
        {
            _nickname = string.IsNullOrWhiteSpace(nickname) ? "Player" : nickname;
            _color = color;
        }

        [Server]
        public void ServerSetAliveState(bool alive)
        {
            _isAlive = alive;

            if (!alive)
                _inventory?.ClearConsumables();
            
            if (_characterController)
                _characterController.enabled = alive;
        }

        [Server]
        public void ServerRespawnAt(Vector3 position, Quaternion rotation)
        {
            if (_characterController)
                _characterController.enabled = false;

            transform.SetPositionAndRotation(position, rotation);

            if (_characterController)
                _characterController.enabled = true;

            _serverPosition = position;
            _serverYaw = rotation.eulerAngles.y;
            _serverPitch = 0f;
        }

        [Command]
        private void CmdSendInput(PlayerNetworkInput input)
        {
            if (!_isAlive)
                return;

            if (!_lookController || !_motor)
                return;

            _lookController.SetYaw(input.Yaw);
            _lookController.SetPitch(input.Pitch);

            _motor.Simulate(input.Move, input.JumpPressed, Time.deltaTime);

            _serverPosition = _motor.Position;
            _serverYaw = _lookController.Yaw;
            _serverPitch = _lookController.Pitch;
        }

        private void UpdateLocalPlayer()
        {
            _localController?.Tick();
            _playerView?.SetWeaponPitch(_lookController.Pitch);

            if (!isServer && _motor)
            {
                if (Vector3.Distance(transform.position, _serverPosition) > 0.05f)
                    _motor.Teleport(_serverPosition);
            }
        }

        private void UpdateRemotePlayer()
        {
            _remoteInterpolator?.SetTarget(_serverPosition, _serverYaw, _serverPitch);
            
            _presentation?.SetWeaponPitch(_serverPitch);
        }

        private void OnNicknameChanged(string oldNickname, string newNickname)
        {
            _presentation?.SetNickname(newNickname);
        }

        private void OnColorChanged(Color oldColor, Color newColor)
        {
            _presentation?.SetColor(newColor);
        }

        private void OnAliveChanged(bool oldAlive, bool newAlive)
        {
            if (isLocalPlayer)
            {
                _localController?.SetControlEnabled(newAlive);
                
                if (_hudView != null)
                {
                    if (newAlive)
                        _hudView.Show();
                    else
                        _hudView.Hide();
                }
            }

            _presentation?.SetAlive(newAlive);
            _playerView?.SetLocalState(isLocalPlayer);
        }
        
        private void OnDestroy()
        {
            _localController?.Dispose();
            _localController = null;
            _hudPresenter?.Dispose();
        }
    }
}