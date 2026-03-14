using System;
using _Project._Scripts.Gameplay.Camera;
using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Gameplay.Movement;
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

        private InputActionsPlayerInputReader _inputReader;
        private LocalPlayerMovementInputController _movementInputController;
        private LocalPlayerCombatController _combatController;

        private bool _initialized;
        private bool _disposed;
        private bool _controlEnabled = true;

        public LocalGamePlayerController(
            GamePlayerView view,
            MouseLookController lookController,
            RemotePlayerInterpolator remoteInterpolator,
            Weapon weapon)
        {
            _view = view;
            _lookController = lookController;
            _remoteInterpolator = remoteInterpolator;
            _weapon = weapon;
        }

        public void Initialize(Action<PlayerNetworkInput> sendInput)
        {
            if (_initialized)
                return;

            if (_lookController == null)
            {
                Debug.LogError("[LocalGamePlayerController] MouseLookController is not assigned.");
                return;
            }

            _initialized = true;
            _controlEnabled = true;

            _view?.SetLocalState(true);

            _inputReader = new InputActionsPlayerInputReader();

            _movementInputController = new LocalPlayerMovementInputController(
                _inputReader,
                _lookController,
                sendInput);

            _combatController = new LocalPlayerCombatController(
                _inputReader,
                _view,
                _lookController,
                _weapon);

            if (_remoteInterpolator != null)
                _remoteInterpolator.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void SetControlEnabled(bool enabled)
        {
            _controlEnabled = enabled;
            Cursor.lockState = enabled ? CursorLockMode.Locked : CursorLockMode.None;
            Cursor.visible = !enabled;
        }

        public void Tick()
        {
            if (!_initialized || _disposed || !_controlEnabled)
                return;

            _movementInputController?.Tick();
            _combatController?.Tick();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _disposed = true;

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            _inputReader?.Dispose();
            _inputReader = null;
            _movementInputController = null;
            _combatController = null;
        }
    }
}