using UnityEngine;

namespace _Project._Scripts.Interfaces
{
    public interface IPlayerStateBroadcaster
    {
        void SetTarget(Vector3 position, float yaw, float pitch);
    }
}