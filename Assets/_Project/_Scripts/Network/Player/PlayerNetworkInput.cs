using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    public struct PlayerNetworkInput
    {
        public Vector2 Move;
        public bool JumpPressed;
        public float Yaw;
        public float Pitch;
    }
}