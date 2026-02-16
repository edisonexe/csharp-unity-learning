using UnityEngine;

namespace _Scripts.Interfaces
{
    public interface IMovementService
    {
        void Move(Vector3 direction, float deltaTime);
    }
}