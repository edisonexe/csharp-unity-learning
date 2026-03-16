using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Gameplay.Inventory;
using _Project._Scripts.Gameplay.Items;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Player
{
    [RequireComponent(typeof(GamePlayer))]
    [RequireComponent(typeof(PlayerInventory))]
    [RequireComponent(typeof(Health))]
    public sealed class PlayerConsumableService : NetworkBehaviour
    {
        [Header("Medkit")]
        [SerializeField] private int _medkitHealAmount = 35;
        [SerializeField] private float _medkitCooldown = 1.5f;

        [Header("Grenade")]
        [SerializeField] private GrenadeProjectile _grenadeProjectilePrefab;
        [SerializeField] private Transform _grenadeSpawnPoint;
        [SerializeField] private float _grenadeThrowForce = 12f;
        [SerializeField] private float _grenadeThrowCooldown = 1.5f;

        private GamePlayer _player;
        private PlayerInventory _inventory;
        private Health _health;

        private double _nextMedkitUseTime;
        private double _nextGrenadeThrowTime;

        private void Awake()
        {
            _player = GetComponent<GamePlayer>();
            _inventory = GetComponent<PlayerInventory>();
            _health = GetComponent<Health>();

            if (!_player || !_inventory || !_health)
            {
                Debug.LogError("[PlayerConsumableService] Required components are missing.", this);
                enabled = false;
                return;
            }

            if (!_grenadeProjectilePrefab)
            {
                Debug.LogError("[PlayerConsumableService] Grenade projectile prefab is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_grenadeSpawnPoint)
            {
                Debug.LogError("[PlayerConsumableService] Grenade spawn point is not assigned.", this);
                enabled = false;
            }
        }

        public void LocalRequestUseMedkit()
        {
            if (!isLocalPlayer)
                return;

            if (_inventory.MedkitsCount <= 0)
                return;
            
            CmdUseMedkit();
        }

        public void LocalRequestThrowGrenade(Vector3 direction)
        {
            if (!isLocalPlayer)
                return;

            if (_inventory.GrenadesCount <= 0)
                return;

            CmdThrowGrenade(direction.normalized);
        }

        [Command]
        private void CmdUseMedkit()
        {
            if (!_player.IsAlive)
                return;

            if (NetworkTime.time < _nextMedkitUseTime)
                return;

            if (_inventory.MedkitsCount <= 0)
                return;

            if (_health.CurrentHp >= _health.MaxHp)
                return;

            if (!_inventory.TryConsumeMedkit())
                return;

            int restoredHp = _health.Restore(_medkitHealAmount);
            _nextMedkitUseTime = NetworkTime.time + _medkitCooldown;

            Debug.Log($"[Medkit] {_player.name} used medkit and restored {restoredHp} HP");
        }

        [Command]
        private void CmdThrowGrenade(Vector3 direction)
        {
            if (!_player.IsAlive)
                return;

            if (NetworkTime.time < _nextGrenadeThrowTime)
                return;

            if (_inventory.GrenadesCount <= 0)
                return;

            if (!_inventory.TryConsumeGrenade())
                return;

            GrenadeProjectile grenade = Instantiate(
                _grenadeProjectilePrefab,
                _grenadeSpawnPoint.position,
                Quaternion.identity);

            NetworkServer.Spawn(grenade.gameObject);

            grenade.Initialize(_player, direction * _grenadeThrowForce);
            
            _nextGrenadeThrowTime = NetworkTime.time + _grenadeThrowCooldown;

            Debug.Log($"[Grenade] {_player.name} threw grenade");
        }
    }
}