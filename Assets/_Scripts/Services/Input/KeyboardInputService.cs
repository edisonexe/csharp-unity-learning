using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Services.Input
{
    public sealed class KeyboardInputService : IInputService, ILookInputService
    {
        private readonly GameInputActions _actions;

        public KeyboardInputService()
        {
            _actions = new GameInputActions();
            _actions.Enable();
        }

        public Vector2 MoveAxis => _actions.Player.Move.ReadValue<Vector2>();

        public Vector2 LookDelta => _actions.Player.Look.ReadValue<Vector2>();
        public bool RotateHeld => _actions.Player.Rotate.IsPressed();
    }
}