using UnityEngine;

namespace _Scripts.Interfaces.Input
{
    public interface ILookInputService
    {
        bool RotateHeld { get; }
        Vector2 LookDelta { get; }
    }
}