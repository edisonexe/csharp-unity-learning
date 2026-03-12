using System;
using _Project._Scripts.Gameplay;
using _Project._Scripts.Gameplay.Camera;
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

        private InputActionsPlayerInputReader _inputReader;
        private NetworkPlayerMovementController _movementController;

        private bool _initialized;
        private bool _disposed;

        public LocalGamePlayerController(
            GamePlayerView view,
            MouseLookController lookController,
            RemotePlayerInterpolator remoteInterpolator)
        {
            _view = view;
            _lookController = lookController;
            _remoteInterpolator = remoteInterpolator;
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

            _view?.SetLocalState(true);

            _inputReader = new InputActionsPlayerInputReader();
            _movementController = new NetworkPlayerMovementController(
                _inputReader,
                _lookController,
                sendInput);

            if (_remoteInterpolator != null)
                _remoteInterpolator.enabled = false;

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        public void Tick()
        {
            if (!_initialized || _disposed)
                return;

            _movementController?.Tick();
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
            _movementController = null;
        }
    }
}