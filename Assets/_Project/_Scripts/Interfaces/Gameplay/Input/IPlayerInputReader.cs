using UnityEngine;

namespace _Project._Scripts.Interfaces.Gameplay.Input
{
    public interface IPlayerInputReader
    {
        Vector2 Move { get; }
        Vector2 LookDelta { get; }
        bool JumpPressedThisFrame { get; }
        bool FireHeld { get; }
        bool PickupPressedThisFrame { get; }
        bool UseMedkitPressedThisFrame { get; }
        bool ThrowGrenadePressedThisFrame { get; }
    }
}