using UnityEngine;

namespace _Project._Scripts.Interfaces
{
    public interface IMotor
    {
        Vector3 Position { get; }
        bool IsGrounded { get; }

        void Simulate(Vector2 moveInput, bool jumpPressed, float deltaTime);
        void Teleport(Vector3 position);
    }
}