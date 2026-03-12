using System;
using System.Collections.Generic;
using _Project._Scripts.Gameplay;
using _Project._Scripts.Gameplay.Camera;
using _Project._Scripts.Gameplay.Movement;
using _Project._Scripts.Gameplay.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    [RequireComponent(typeof(CharacterController))]
    [RequireComponent(typeof(CharacterControllerMotor))]
    [RequireComponent(typeof(MouseLookController))]
    [RequireComponent(typeof(GamePlayerView))]
    [RequireComponent(typeof(RemotePlayerInterpolator))]
    public sealed class GamePlayer : NetworkBehaviour
    {
        private CharacterController _characterController;
        private CharacterControllerMotor _motor;
        private MouseLookController _lookController;
        private GamePlayerView _view;
        private RemotePlayerInterpolator _remoteInterpolator;

        [SyncVar(hook = nameof(OnNicknameChanged))]
        private string _nickname;

        [SyncVar(hook = nameof(OnColorChanged))]
        private Color _color = Color.white;

        [SyncVar] private Vector3 _serverPosition;
        [SyncVar] private float _serverYaw;
        [SyncVar] private float _serverPitch;

        private static readonly List<GamePlayer> _players = new();

        public static event Action PlayerCountChanged;
        public static int PlayerCount => _players.Count;

        private GamePlayerPresentation _presentation;
        private LocalGamePlayerController _localController;

        private void Awake()
        {
            _characterController = GetComponent<CharacterController>();
            _motor = GetComponent<CharacterControllerMotor>();
            _lookController = GetComponent<MouseLookController>();
            _view = GetComponent<GamePlayerView>();
            _remoteInterpolator = GetComponent<RemotePlayerInterpolator>();

            if (!_characterController || !_motor || !_lookController || !_view || !_remoteInterpolator)
            {
                Debug.LogError("[GamePlayer] Required components are missing.", this);
                enabled = false;
                return;
            }

            _motor.Construct(_characterController);
            _presentation = new GamePlayerPresentation(_view);
        }

        public override void OnStartClient()
        {
            base.OnStartClient();

            if (!_players.Contains(this))
                _players.Add(this);

            PlayerCountChanged?.Invoke();
            _presentation?.Apply(_nickname, _color, isLocalPlayer);
        }

        public override void OnStopClient()
        {
            _players.Remove(this);
            PlayerCountChanged?.Invoke();

            base.OnStopClient();
        }

        public override void OnStartLocalPlayer()
        {
            if (!enabled)
                return;

            _localController ??= new LocalGamePlayerController(
                _view,
                _lookController,
                _remoteInterpolator);

            _localController.Initialize(CmdSendInput);
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

        [Command]
        private void CmdSendInput(PlayerNetworkInput input)
        {
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

            if (!isServer && _motor)
            {
                if (Vector3.Distance(transform.position, _serverPosition) > 0.05f)
                    _motor.Teleport(_serverPosition);
            }
        }

        private void UpdateRemotePlayer()
        {
            _remoteInterpolator?.SetTarget(_serverPosition, _serverYaw, _serverPitch);
        }

        private void OnNicknameChanged(string oldNickname, string newNickname)
        {
            _presentation?.SetNickname(newNickname);
        }

        private void OnColorChanged(Color oldColor, Color newColor)
        {
            _presentation?.SetColor(newColor);
        }

        private void OnDestroy()
        {
            _localController?.Dispose();
            _localController = null;
        }
    }
}