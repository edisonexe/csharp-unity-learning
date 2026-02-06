using Characters.Enemy;
using UnityEngine;
using Weapon;

namespace Characters.Player.Controllers
{
    public sealed class PlayerShootController
    {
        private readonly Transform _shootingPoint;
        private readonly LayerMask _shootMask;
        private readonly ShootTracerView _tracerPrefab;

        public PlayerShootController(Transform shootingPoint, LayerMask shootMask, ShootTracerView tracerPrefab)
        {
            _shootingPoint = shootingPoint;
            _shootMask = shootMask;
            _tracerPrefab = tracerPrefab;
        }

        public void Update(Input.PlayerInputReader input, WeaponModel weapon, PlayerAimController aim, 
            Transform playerTransform)
        {
            if (!input || weapon == null || aim == null || !playerTransform) return;
            if (!_shootingPoint) return;

            bool wantShoot =
                weapon.Config.FireMode == FireMode.Auto
                    ? input.ShootHeld
                    : input.ShootPressedThisFrame;

            if (!wantShoot) return;
            if (weapon.Ammo <= 0) return;
            if (!weapon.TryConsumeShot(Time.time)) return;

            if (!aim.TryGetAimPoint(input.MousePos, playerTransform.position.y, weapon.Config.Range, out var aimPoint))
                return;

            Vector3 from = _shootingPoint.position;
            Vector3 dir = (aimPoint - from);
            if (dir.sqrMagnitude < 0.0001f) return;
            dir.Normalize();

            float range = weapon.Config.Range;
            int damage = weapon.Config.Damage;

            Vector3 to = from + dir * range;

            if (Physics.Raycast(from, dir, out RaycastHit hit, range, _shootMask))
            {
                to = hit.point;
                hit.collider.GetComponent<EnemyController>()?.TakeDamage(damage);
            }

            if (_tracerPrefab)
            {
                var tracer = Object.Instantiate(_tracerPrefab);
                tracer.Show(from, to);
            }
        }
    }
}
