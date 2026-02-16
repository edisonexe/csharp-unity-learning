using UnityEngine;

namespace _Scripts.Interfaces
{
    public interface ILookInputService
    {
        bool RotateHeld { get; }
        Vector2 LookDelta { get; }
    }
}