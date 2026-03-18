using System;
using System.Collections.Generic;
using _Project._Scripts.Gameplay.Camera;
using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Gameplay.Inventory;
using _Project._Scripts.Gameplay.Match;
using _Project._Scripts.Gameplay.Movement;
using _Project._Scripts.Gameplay.Player;
using _Project._Scripts.Interfaces.Views;
using _Project._Scripts.UI.Controllers;
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
        private const float POSITION_SNAP_DISTANCE = 2f;
        private const float POSITION_LERP_THRESHOLD = 0.01f;
        private const float POSITION_LERP_SPEED = 15f;

        private static readonly List<GamePlayer> _players = new();

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

        private MatchManager _matchManager;
        private bool _isRegisteredInMatch;

        private PlayerNetworkInput _cachedServerInput;
        private bool _hasCachedServerInput;

        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;

        [SyncVar(hook = nameof(OnColorChanged))]
        private Color _color = Color.white;

        [SyncVar(hook = nameof(OnAliveChanged))]
        private bool _isAlive = true;

        [SyncVar(hook = nameof(OnGameplayBlockedChanged))]
        private bool _isGameplayBlocked;

        [SyncVar]
        private Vector3 _serverPosition;

        [SyncVar]
        private float _serverYaw;

        [SyncVar]
        private float _serverPitch;

        public Health Health => _health;
        public Weapon Weapon => _weapon;
        public string Nickname => _nickname;
        public bool IsAlive => _isAlive;
        public bool IsGameplayBlocked => _isGameplayBlocked;
        public MatchManager MatchManager => _matchManager;
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

            if (!_characterController ||
                !_motor ||
                !_lookController ||
                !_playerView ||
                !_remoteInterpolator ||
                !_health ||
                !_weapon ||
                !_inventory ||
                !_itemCollector ||
                !_consumableService)
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

        [Server]
        public void ConstructMatch(MatchManager matchManager)
        {
            if (!matchManager)
            {
                Debug.LogError("[GamePlayer] ConstructMatch received null MatchManager.", this);
                return;
            }

            _matchManager = matchManager;
            RegisterInMatchIfNeeded();
        }

        [Server]
        public void EnsureMatchManagerAssigned()
        {
            if (_matchManager)
                return;

            _matchManager = FindFirstObjectByType<MatchManager>();

            if (!_matchManager)
            {
                Debug.LogError($"[GamePlayer] MatchManager not found for player netId={netId}.", this);
                return;
            }

            RegisterInMatchIfNeeded();
        }

        [Server]
        private void RegisterInMatchIfNeeded()
        {
            if (!_matchManager || _isRegisteredInMatch)
                return;

            _matchManager.RegisterPlayer(this);
            _isRegisteredInMatch = true;
        }

        [Server]
        private void UnregisterFromMatchIfNeeded()
        {
            if (!_isRegisteredInMatch)
                return;

            if (_matchManager)
                _matchManager.UnregisterPlayer(this);

            _isRegisteredInMatch = false;
        }

        public override void OnStartServer()
        {
            base.OnStartServer();

            gameObject.name = $"GamePlayer_{netId}";
            _isGameplayBlocked = false;

            EnsureMatchManagerAssigned();
        }

        public override void OnStopServer()
        {
            UnregisterFromMatchIfNeeded();
            base.OnStopServer();
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

            _localController ??= new LocalGamePlayerController(
                _playerView,
                _lookController,
                _remoteInterpolator,
                _weapon,
                _itemCollector,
                _consumableService);

            _localController.Initialize(SendInput);

            _playerView.SetLocalState(true);
            _playerView.SetAliveState(_isAlive);

            LocalPlayerSpawned?.Invoke(this);
        }

        private void Update()
        {
            if (!enabled)
                return;

            if (isLocalPlayer)
                PreUpdateLocalPlayer();

            if (isServer)
                ServerSimulateMovement();

            if (isLocalPlayer)
            {
                PostUpdateLocalPlayer();
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
            {
                _inventory?.ClearConsumables();
                ClearCachedServerInput();
            }

            if (_characterController)
                _characterController.enabled = alive;
        }

        [Server]
        public void ServerSetGameplayBlocked(bool blocked)
        {
            _isGameplayBlocked = blocked;

            if (blocked)
                ClearCachedServerInput();
        }

        [Server]
        public void ServerRespawnAt(Vector3 position, Quaternion rotation)
        {
            ClearCachedServerInput();

            if (_characterController)
                _characterController.enabled = false;

            transform.SetPositionAndRotation(position, rotation);

            if (_characterController)
                _characterController.enabled = true;

            _serverPosition = position;
            _serverYaw = rotation.eulerAngles.y;
            _serverPitch = 0f;
        }

        private void PreUpdateLocalPlayer()
        {
            _localController?.Tick();

            if (_playerView && _lookController)
                _playerView.SetWeaponPitch(_lookController.Pitch);
        }

        private void PostUpdateLocalPlayer()
        {
            if (!isServer)
                ReconcileLocalClient();
        }

        private void ReconcileLocalClient()
        {
            if (!_motor)
                return;

            float distance = Vector3.Distance(transform.position, _serverPosition);

            if (distance > POSITION_SNAP_DISTANCE)
            {
                _motor.Teleport(_serverPosition);
            }
            else if (distance > POSITION_LERP_THRESHOLD)
            {
                transform.position = Vector3.Lerp(
                    transform.position,
                    _serverPosition,
                    Time.deltaTime * POSITION_LERP_SPEED);
            }
        }

        [ServerCallback]
        private void ServerSimulateMovement()
        {
            if (!_isAlive || _isGameplayBlocked)
                return;

            if (!_lookController || !_motor)
                return;

            if (!_hasCachedServerInput)
                return;

            bool jumpPressed = _cachedServerInput.JumpPressed;

            _lookController.SetYaw(_cachedServerInput.Yaw);
            _lookController.SetPitch(_cachedServerInput.Pitch);

            _motor.Simulate(
                _cachedServerInput.Move,
                jumpPressed,
                Time.deltaTime);

            _serverPosition = _motor.Position;
            _serverYaw = _lookController.Yaw;
            _serverPitch = _lookController.Pitch;
            
            _cachedServerInput.JumpPressed = false;
        }

        private void SendInput(PlayerNetworkInput input)
        {
            if (!_isAlive || _isGameplayBlocked)
                return;

            if (isServer)
            {
                _cachedServerInput.Move = input.Move;
                _cachedServerInput.Yaw = input.Yaw;
                _cachedServerInput.Pitch = input.Pitch;

                if (input.JumpPressed)
                    _cachedServerInput.JumpPressed = true;

                _hasCachedServerInput = true;
                return;
            }

            CmdSendInput(input);
        }

        [Command]
        private void CmdSendInput(PlayerNetworkInput input)
        {
            if (!_isAlive || _isGameplayBlocked)
                return;

            _cachedServerInput.Move = input.Move;
            _cachedServerInput.Yaw = input.Yaw;
            _cachedServerInput.Pitch = input.Pitch;

            if (input.JumpPressed)
                _cachedServerInput.JumpPressed = true;

            _hasCachedServerInput = true;
        }

        private void ClearCachedServerInput()
        {
            _cachedServerInput = default;
            _hasCachedServerInput = false;
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
                bool canControl = newAlive && !_isGameplayBlocked;
                _localController?.SetControlEnabled(canControl);

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

        private void OnGameplayBlockedChanged(bool oldValue, bool newValue)
        {
            if (!isLocalPlayer)
                return;

            bool canControl = _isAlive && !newValue;
            _localController?.SetControlEnabled(canControl);
        }

        private void OnDestroy()
        {
            _localController?.Dispose();
            _localController = null;

            _hudPresenter?.Dispose();
            _hudPresenter = null;
        }
    }
}