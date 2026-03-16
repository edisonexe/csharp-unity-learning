using _Project._Scripts.Gameplay.Items;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Player
{
    [RequireComponent(typeof(GamePlayer))]
    public sealed class PlayerItemCollector : NetworkBehaviour
    {
        private GamePlayer _player;

        private void Awake()
        {
            _player = GetComponent<GamePlayer>();

            if (!_player)
            {
                Debug.LogError("[PlayerItemCollector] GamePlayer is missing.", this);
                enabled = false;
            }
        }

        public void LocalTryPickup(uint itemNetId)
        {
            if (!isLocalPlayer)
                return;

            CmdTryPickup(itemNetId);
        }

        [Command]
        private void CmdTryPickup(uint itemNetId)
        {
            if (!_player.IsAlive)
                return;

            if (!NetworkServer.spawned.TryGetValue(itemNetId, out NetworkIdentity identity))
                return;

            PickupItem item = identity.GetComponent<PickupItem>();
            if (!item)
                return;

            item.TryPickup(_player);
        }
    }
}