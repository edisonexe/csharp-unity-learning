using _Project._Scripts.Gameplay.Inventory;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Items
{
    public sealed class PickupItem : NetworkBehaviour
    {
        [SerializeField] private ItemType _itemType;
        [SerializeField] private float _pickupRadius = 2f;

        [SyncVar]
        private bool _isAvailable = true;

        private ItemSpawner _spawner;
        private Transform _spawnPoint;

        public ItemType ItemType => _itemType;
        public bool IsAvailable => _isAvailable;

        [Server]
        public void Initialize(ItemSpawner spawner, Transform spawnPoint, ItemType itemType)
        {
            _spawner = spawner;
            _spawnPoint = spawnPoint;
            _itemType = itemType;
            _isAvailable = true;
        }

        [Server]
        public bool TryPickup(GamePlayer player)
        {
            if (!player || !_isAvailable)
                return false;

            float sqrDistance = (player.transform.position - transform.position).sqrMagnitude;
            if (sqrDistance > _pickupRadius * _pickupRadius)
                return false;

            PlayerInventory inventory = player.GetComponent<PlayerInventory>();
            if (!inventory)
                return false;

            inventory.AddItem(_itemType);
            _isAvailable = false;

            Debug.Log($"[PickupItem] {player.name} picked up {_itemType}");

            if (_spawner && _spawnPoint)
                _spawner.NotifyItemPicked(_spawnPoint, _itemType);

            NetworkServer.Destroy(gameObject);
            return true;
        }
    }
}