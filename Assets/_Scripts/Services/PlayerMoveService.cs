using UnityEngine;

namespace Services
{
    public sealed class PlayerMoveService
    {
        private readonly float _speed;

        public PlayerMoveService(float speed) => _speed = speed;

        public Vector3 CalculateDelta(Vector2 input, float dt)
        {
            if (input.sqrMagnitude < 0.0001f) return Vector3.zero;

            var dir = new Vector3(input.x, 0f, input.y).normalized;
            return dir * (_speed * dt);
        }
    }
}