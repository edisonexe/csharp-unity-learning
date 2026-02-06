using Input;
using UnityEngine;

namespace Characters.Player.Controllers
{
    public sealed class PlayerAimController
    {
        private readonly UnityEngine.Camera _camera;
        private readonly LayerMask _aimMask;

        public PlayerAimController(UnityEngine.Camera camera, LayerMask aimMask)
        {
            _camera = camera;
            _aimMask = aimMask;
        }

        public void Update(PlayerInputReader input, Transform playerTransform, Weapon.WeaponModel weapon)
        {
            if (!input || !playerTransform || weapon == null) return;

            if (!TryGetAimPoint(input.MousePos, playerTransform.position.y, weapon.Config.Range, out Vector3 aimPoint))
                return;

            Vector3 look = aimPoint - playerTransform.position;
            look.y = 0f;

            if (look.sqrMagnitude > 0.0001f)
                playerTransform.forward = look.normalized;
        }

        public bool TryGetAimPoint(Vector2 mousePos, float planeY, float range, out Vector3 aimPoint)
        {
            aimPoint = default;
            if (!_camera) return false;

            Ray ray = _camera.ScreenPointToRay(mousePos);

            if (Physics.Raycast(ray, out RaycastHit hit, range, _aimMask))
            {
                aimPoint = hit.point;
                return true;
            }

            Plane plane = new Plane(Vector3.up, new Vector3(0f, planeY, 0f));
            if (plane.Raycast(ray, out float t))
            {
                aimPoint = ray.GetPoint(t);
                return true;
            }

            return false;
        }
    }
}