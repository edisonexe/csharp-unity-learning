using UnityEngine;

namespace _Project._Scripts.Interfaces.Gameplay.Network
{
    public interface IPlayerStateBroadcaster
    {
        void SetTarget(Vector3 position, float yaw, float pitch);
    }
}