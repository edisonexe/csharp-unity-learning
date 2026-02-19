using _Scripts.Interfaces.Input;
using UnityEngine;

namespace _Scripts.Services.Input
{
    public sealed class FakeInputService : IInputService, IInputToggle
    {
        private readonly Vector2 _fixed;
        private bool _enabled = true;
        
        public FakeInputService(Vector2 fixedDirection) => _fixed = fixedDirection;

        public Vector2 MoveAxis => _enabled ? _fixed  : Vector2.zero;
        public void Enable() => _enabled = true;

        public void Disable() => _enabled = false;
    }
}