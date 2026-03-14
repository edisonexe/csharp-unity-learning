using _Project._Scripts.Configs;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Combat
{
    public class Weapon : NetworkBehaviour
    {
        [SerializeField] private WeaponConfig _cfg;
        [SerializeField] private LayerMask _hitMask = ~0;
        [SerializeField] private Transform _muzzlePoint;
    
        [Header("Local Effect")]
        [SerializeField] private AudioSource _shotAudio;
        [SerializeField] private AudioClip _shotClip;

        [Header("Shared Effect")]
        [SerializeField] private LineRenderer _tracerPrefab;
        [SerializeField] private float _tracerLifetime = 0.05f;
    
        private GamePlayer _player;
        private double _nextFireTime;
    
        private void Awake()
        {
            _player = GetComponent<GamePlayer>();
        
            if (!_player)
            {
                Debug.LogError("[Weapon] GamePlayer component not found.", this);
                enabled = false;
                return;
            }

            if (!_cfg)
            {
                Debug.LogError("[Weapon] WeaponConfig is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_shotAudio)
            {
                Debug.LogError("[Weapon] Shot AudioSource is not assigned.", this);
                enabled = false;
                return;
            }

            if (!_tracerPrefab)
            {
                Debug.LogError("[Weapon] Tracer prefab is not assigned.", this);
                enabled = false;
                return;
            }
        
            if (!_muzzlePoint)
            {
                Debug.LogError("[Weapon] MuzzlePoint is not assigned.", this);
                enabled = false;
                return;
            }
        }

        public Vector3 GetMuzzlePosition() => _muzzlePoint.position;
    
        public void LocalRequestShoot(Vector3 direction)
        {
            if (!isLocalPlayer)
                return;

            if (!_player || !_player.IsAlive)
                return;

            if (direction.sqrMagnitude <= 0.0001f)
                return;

            PlayLocalShotSound();
        
            CmdShoot(direction.normalized);
        }

        private void PlayLocalShotSound()
        {
            if (_shotAudio && _shotClip)
                _shotAudio.PlayOneShot(_shotClip);
        }
    
        [Command]
        private void CmdShoot(Vector3 direction)
        {
            if (!_player || !_player.IsAlive)
                return;

            if (NetworkTime.time < _nextFireTime)
                return;

            _nextFireTime = NetworkTime.time + _cfg.FireRate;

            Vector3 origin = GetServerShotOrigin();
            Vector3 shootDirection = direction.normalized;

            bool didHit = Physics.Raycast(
                origin,
                shootDirection,
                out RaycastHit hit,
                _cfg.Range,
                _hitMask,
                QueryTriggerInteraction.Ignore);

            Vector3 hitPoint = didHit
                ? hit.point
                : origin + shootDirection * _cfg.Range;

            if (didHit)
            {
                Health targetHealth = hit.collider.GetComponentInParent<Health>();

                if (targetHealth && targetHealth != _player.Health)
                    targetHealth.TakeDamage(_cfg.Damage, _player);
            }

            RpcSpawnTracer(origin, hitPoint);
        }

        [Server] private Vector3 GetServerShotOrigin() => _muzzlePoint.position;

        [ClientRpc]
        private void RpcSpawnTracer(Vector3 origin, Vector3 hitPoint)
        {
            SpawnTracer(origin, hitPoint);
        }

        private void SpawnTracer(Vector3 origin, Vector3 hitPoint)
        {
            if (!_tracerPrefab)
                return;

            LineRenderer tracer = Instantiate(_tracerPrefab);
            tracer.SetPosition(0, origin);
            tracer.SetPosition(1, hitPoint);

            Destroy(tracer.gameObject, _tracerLifetime);
        }
    }
}