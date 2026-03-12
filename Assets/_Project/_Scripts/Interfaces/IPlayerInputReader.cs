using UnityEngine;

namespace _Project._Scripts.Interfaces
{
    public interface IPlayerInputReader
    {
        Vector2 Move { get; }
        Vector2 LookDelta { get; }
        bool JumpPressedThisFrame { get; }
    }
}