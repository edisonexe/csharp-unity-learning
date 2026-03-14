using System;
using _Project._Scripts.Interfaces;
using UnityEngine;

namespace _Project._Scripts.Input
{
    public sealed class InputActionsPlayerInputReader : IPlayerInputReader, IDisposable
    {
        private readonly PlayerInputActions _actions;
        private bool _disposed;

        public Vector2 Move => _actions.Player.Move.ReadValue<Vector2>();
        public Vector2 LookDelta => _actions.Player.Look.ReadValue<Vector2>();
        public bool JumpPressedThisFrame => _actions.Player.Jump.triggered;
        public bool FireHeld => _actions.Player.Fire.IsPressed();
        
        public InputActionsPlayerInputReader()
        {
            _actions = new PlayerInputActions();
            _actions.Enable();
        }

        public void Dispose()
        {
            if (_disposed)
                return;

            _actions.Disable();
            _disposed = true;
        }
    }
}