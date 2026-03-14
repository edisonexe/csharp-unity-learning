using _Project._Scripts.Gameplay.Camera;
using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Gameplay.Player;
using _Project._Scripts.Input;
using UnityEngine;

namespace _Project._Scripts.Network.Player
{
    public sealed class LocalPlayerCombatController
    {
        private const float AimDistance = 1000f;

        private readonly InputActionsPlayerInputReader _inputReader;
        private readonly GamePlayerView _view;
        private readonly MouseLookController _lookController;
        private readonly Weapon _weapon;

        public LocalPlayerCombatController(
            InputActionsPlayerInputReader inputReader,
            GamePlayerView view,
            MouseLookController lookController,
            Weapon weapon)
        {
            _inputReader = inputReader;
            _view = view;
            _lookController = lookController;
            _weapon = weapon;
        }

        public void Tick()
        {
            if (_inputReader == null || _weapon == null)
                return;

            if (!_inputReader.FireHeld)
                return;

            Vector3 direction = GetShootDirection();
            _weapon.LocalRequestShoot(direction);
        }

        private Vector3 GetShootDirection()
        {
            Camera cam = _view != null ? _view.PlayerCamera : null;

            if (cam == null)
                return _lookController.transform.forward;

            Ray ray = cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            Vector3 targetPoint;
            if (Physics.Raycast(ray, out RaycastHit hit, AimDistance, ~0, QueryTriggerInteraction.Ignore))
                targetPoint = hit.point;
            else
                targetPoint = ray.origin + ray.direction * AimDistance;

            Vector3 muzzle = _weapon.GetMuzzlePosition();
            return (targetPoint - muzzle).normalized;
        }
    }
}