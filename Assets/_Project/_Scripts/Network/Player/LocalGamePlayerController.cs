using System;
using _Project._Scripts.Gameplay.Camera;
using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Gameplay.Player;
using _Project._Scripts.Input;
using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    public sealed class LocalGamePlayerController : IDisposable
    {
        private readonly GamePlayerView _view;
        private readonly MouseLookController _lookController;
        private readonly RemotePlayerInterpolator _remoteInterpolator;
        private readonly Weapon _weapon;
        private readonly PlayerItemCollector _itemCollector;
        private readonly PlayerConsumableService _consumableService;

        private InputActionsPlayerInputReader _inputReader;
        private LocalPlayerMovementInputController _movementInputController;
        private LocalPlayerCombatController _combatController;
        private LocalPlayerItemsController _itemsController;

        private Action<PlayerNetworkInput> _sendInput;

        private bool _initialized;
        private bool _disposed;
        private bool _controlEnabled;

        public Vector3 MoveInput { get; private set; }
        public bool JumpPressed { get; private set; }

        public LocalGamePlayerController(
            GamePlayerView view,
            MouseLookController lookController,
            RemotePlayerInterpolator remoteInterpolator,
            Weapon weapon,
            PlayerItemCollector itemCollector,
            PlayerConsumableService consumableService)
        {
            _view = view;
            _lookController = lookController;
            _remoteInterpolator = remoteInterpolator;
            _weapon = weapon;
            _itemCollector = itemCollector;
            _consumableService = consumableService;
        }

        public void Initialize(Action<PlayerNetworkInput> sendInput)
        {
            if (_initialized)
                return;

            if (!ValidateDependencies(sendInput))
                return;

            _sendInput = sendInput;
            _inputReader = new InputActionsPlayerInputReader();

            _movementInputController = new LocalPlayerMovementInputController(
                _inputReader,
                _lookController,
                _sendInput);

            _combatController = new LocalPlayerCombatController(
                _inputReader,
                _view,
                _lookController,
                _weapon);

            _itemsController = new LocalPlayerItemsController(
                _inputReader,
                _itemCollector,
                _consumableService,
                _view.PlayerCamera);

            _initialized = true;
            _controlEnabled = true;

            _view.SetLocalState(true);

            if (_remoteInterpolator)
                _remoteInterpolator.enabled = false;

            SetCursorState(true);
            ResetCachedInput();
        }

        public void SetControlEnabled(bool enabled)
        {
            if (_disposed)
                return;

            _controlEnabled = enabled;

            if (!enabled)
                ResetCachedInput();

            SetCursorState(enabled);
        }

        public void Tick()
        {
            if (!_initialized || _disposed)
            {
                ResetCachedInput();
                return;
            }

            if (!_controlEnabled)
            {
                ResetCachedInput();
                return;
            }

            UpdateCachedInput();

            _movementInputController?.Tick();
            _combatController?.Tick();
            _itemsController?.Tick();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            ResetCachedInput();
            SetCursorState(false);

            _inputReader?.Dispose();
            _inputReader = null;

            _movementInputController = null;
            _combatController = null;
            _itemsController = null;
            _sendInput = null;
        }

        private bool ValidateDependencies(Action<PlayerNetworkInput> sendInput)
        {
            if (!_view)
            {
                Debug.LogError("[LocalGamePlayerController] GamePlayerView is missing.");
                return false;
            }

            if (!_lookController)
            {
                Debug.LogError("[LocalGamePlayerController] MouseLookController is missing.");
                return false;
            }

            if (!_weapon)
            {
                Debug.LogError("[LocalGamePlayerController] Weapon is missing.");
                return false;
            }

            if (!_itemCollector)
            {
                Debug.LogError("[LocalGamePlayerController] PlayerItemCollector is missing.");
                return false;
            }

            if (!_consumableService)
            {
                Debug.LogError("[LocalGamePlayerController] PlayerConsumableService is missing.");
                return false;
            }

            if (_view.PlayerCamera == null)
            {
                Debug.LogError("[LocalGamePlayerController] Player camera is missing.");
                return false;
            }

            if (sendInput == null)
            {
                Debug.LogError("[LocalGamePlayerController] Send input callback is null.");
                return false;
            }

            return true;
        }

        private void UpdateCachedInput()
        {
            Vector2 move = _inputReader.Move;

            if (move.sqrMagnitude > 1f)
                move.Normalize();

            MoveInput = new Vector3(move.x, 0f, move.y);
            JumpPressed = _inputReader.JumpPressedThisFrame;
        }

        private void ResetCachedInput()
        {
            MoveInput = Vector3.zero;
            JumpPressed = false;
        }

        private static void SetCursorState(bool locked)
        {
            Cursor.lockState = locked
                ? CursorLockMode.Locked
                : CursorLockMode.None;

            Cursor.visible = !locked;
        }
    }
}