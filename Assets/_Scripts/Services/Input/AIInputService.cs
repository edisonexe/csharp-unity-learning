using _Scripts.Interfaces.Input;
using UnityEngine;

namespace _Scripts.Services.Input
{
    public sealed class AIInputService : IInputService, IInputToggle
    {
        private readonly Transform _target;
        private readonly Transform _self;
        private readonly float _stopDistance;
        private bool _enabled = true;
        
        public AIInputService(Transform self, Transform target, float stopDistance = 0.3f)
        {
            _self = self;
            _target = target;
            _stopDistance = Mathf.Max(0.01f, stopDistance);
        }

        public Vector2 MoveAxis
        {
            get
            {
                if (!_enabled || !_target || !_self)
                    return Vector2.zero;

                var delta = _target.position - _self.position;
                delta.y = 0f;

                if (delta.sqrMagnitude <= _stopDistance * _stopDistance)
                    return Vector2.zero;

                var dir = delta.normalized;
                return new Vector2(dir.x, dir.z);
            }
        }

        public void Enable() => _enabled = true;

        public void Disable() => _enabled = false;
    }
}



