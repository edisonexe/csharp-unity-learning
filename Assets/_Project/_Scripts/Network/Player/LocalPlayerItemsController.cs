using _Project._Scripts.Gameplay.Items;
using _Project._Scripts.Gameplay.Player;
using _Project._Scripts.Interfaces;
using _Project._Scripts.Interfaces.Gameplay.Input;
using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    public sealed class LocalPlayerItemsController
    {
        private readonly IPlayerInputReader _inputReader;
        private readonly PlayerItemCollector _itemCollector;
        private readonly PlayerConsumableService _consumableService;
        private readonly Camera _camera;

        public LocalPlayerItemsController(IPlayerInputReader inputReader, PlayerItemCollector itemCollector,
            PlayerConsumableService consumableService, Camera camera)
        {
            _inputReader = inputReader;
            _itemCollector = itemCollector;
            _consumableService = consumableService;
            _camera = camera;
        }

        public void Tick()
        {
            if (_inputReader.UseMedkitPressedThisFrame)
                _consumableService.LocalRequestUseMedkit();

            if (_inputReader.ThrowGrenadePressedThisFrame)
                _consumableService.LocalRequestThrowGrenade(_camera.transform.forward);

            if (_inputReader.PickupPressedThisFrame)
                TryPickup();
        }

        private void TryPickup()
        {
            Ray ray = new Ray(_camera.transform.position, _camera.transform.forward);

            if (!Physics.Raycast(ray, out RaycastHit hit, 3f))
                return;

            PickupItem item = hit.collider.GetComponentInParent<PickupItem>();
            if (!item)
                return;

            _itemCollector.LocalTryPickup(item.netId);
        }
    }
}