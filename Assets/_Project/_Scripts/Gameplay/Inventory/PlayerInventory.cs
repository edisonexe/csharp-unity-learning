using System;
using _Project._Scripts.Gameplay.Items;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Gameplay.Inventory;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Inventory
{
    [RequireComponent(typeof(_Project._Scripts.Network.Player.GamePlayer))]
    public sealed class PlayerInventory : NetworkBehaviour, IPlayerInventory
    {
        [SyncVar(hook = nameof(OnMedkitsChanged))]
        private int _medkitsCount;

        [SyncVar(hook = nameof(OnGrenadesChanged))]
        private int _grenadesCount;

        public int MedkitsCount => _medkitsCount;
        public int GrenadesCount => _grenadesCount;

        public event Action<int> MedkitsCountChanged;
        public event Action<int> GrenadesCountChanged;

        [Server]
        public void AddItem(ItemType itemType, int amount = 1)
        {
            if (amount <= 0)
                return;

            switch (itemType)
            {
                case ItemType.Medkit:
                    _medkitsCount += amount;
                    break;

                case ItemType.Grenade:
                    _grenadesCount += amount;
                    break;
            }
        }

        [Server]
        public bool TryConsumeMedkit()
        {
            if (_medkitsCount <= 0)
                return false;

            _medkitsCount--;
            return true;
        }

        [Server]
        public bool TryConsumeGrenade()
        {
            if (_grenadesCount <= 0)
                return false;

            _grenadesCount--;
            return true;
        }

        [Server]
        public void ClearConsumables()
        {
            _medkitsCount = 0;
            _grenadesCount = 0;
        }
        
        private void OnMedkitsChanged(int oldValue, int newValue)
        {
            if (isLocalPlayer)
                MedkitsCountChanged?.Invoke(newValue);
        }

        private void OnGrenadesChanged(int oldValue, int newValue)
        {
            if (isLocalPlayer)
                GrenadesCountChanged?.Invoke(newValue);
        }
    }
}