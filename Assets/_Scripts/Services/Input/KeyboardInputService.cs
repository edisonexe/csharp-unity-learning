using _Scripts.Interfaces.Input;
using UnityEngine;

namespace _Scripts.Services.Input
{
    public sealed class KeyboardInputService : IInputService, ILookInputService, IInputToggle
    {
        private readonly GameInputActions _inputActions;
        private bool _enabled;
        
        public KeyboardInputService(GameInputActions inputActions)
        {
            _inputActions = inputActions;
        }

        public Vector2 MoveAxis => _enabled ? _inputActions.Player.Move.ReadValue<Vector2>() : Vector2.zero;

        public Vector2 LookDelta => _enabled ? _inputActions.Player.Look.ReadValue<Vector2>() : Vector2.zero;
        public bool RotateHeld => _enabled && _inputActions.Player.Rotate.IsPressed();
        
        public void Enable() => _enabled = true;
        public void Disable() => _enabled  = false;
    }
}