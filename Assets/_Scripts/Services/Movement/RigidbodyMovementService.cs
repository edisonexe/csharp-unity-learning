using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Services.Movement
{
    public sealed class RigidbodyMovementService : IMovementService
    {
        private readonly Rigidbody _rb;
        private readonly float _speed;

        public RigidbodyMovementService(Rigidbody rb, float speed)
        {
            _rb = rb;
            _speed = speed;
        }

        public void Move(Vector3 direction, float deltaTime)
        {
            if (!_rb) return;

            var velocity = direction.normalized * _speed;
            velocity.y = _rb.linearVelocity.y;

            _rb.linearVelocity = velocity;
        }
    }
}