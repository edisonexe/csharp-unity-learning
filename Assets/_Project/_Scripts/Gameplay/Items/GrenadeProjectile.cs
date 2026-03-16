using System.Collections;
using _Project._Scripts.Gameplay.Combat;
using _Project._Scripts.Network.Player;
using Mirror;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Items
{
    [RequireComponent(typeof(Rigidbody))]
    public sealed class GrenadeProjectile : NetworkBehaviour
    {
        [SerializeField] private float _fuseTime = 3f;
        [SerializeField] private float _explosionRadius = 5f;
        [SerializeField] private int _maxDamage = 60;
        [SerializeField] private LayerMask _damageMask = ~0;
        [SerializeField] private GameObject _explosionEffectPrefab;

        private Rigidbody _rigidbody;
        private GamePlayer _owner;

        private void Awake()
        {
            _rigidbody = GetComponent<Rigidbody>();

            if (!_rigidbody)
            {
                Debug.LogError("[GrenadeProjectile] Rigidbody is missing.", this);
                enabled = false;
                return;
            }
        }
        
        [Server]
        public void Initialize(GamePlayer owner, Vector3 velocity)
        {
            _owner = owner;
            _rigidbody.linearVelocity = velocity;
            StartCoroutine(ExplosionRoutine());
        }

        [Server]
        private IEnumerator ExplosionRoutine()
        {
            yield return new WaitForSeconds(_fuseTime);
            Explode();
        }

        [Server]
        private void Explode()
        {
            Vector3 explosionPosition = transform.position;

            Collider[] hits = Physics.OverlapSphere(
                explosionPosition,
                _explosionRadius,
                _damageMask,
                QueryTriggerInteraction.Ignore);

            foreach (Collider hit in hits)
            {
                Health health = hit.GetComponentInParent<Health>();
                if (!health)
                    continue;

                float distance = Vector3.Distance(explosionPosition, health.transform.position);
                int damage = CalculateDamage(distance);

                if (damage <= 0)
                    continue;

                Debug.Log($"[GrenadeExplosion] {health.name} received {damage} damage from " +
                          $"{(_owner ? _owner.name : "unknown")} distance={distance:F2} explosionPos={explosionPosition}");

                
                health.TakeDamage(damage, _owner);
            }
            
            if (isClient)
                SpawnExplosionEffectLocal(explosionPosition);
            
            RpcPlayExplosionEffect(explosionPosition);

            NetworkServer.Destroy(gameObject);
        }

        [ClientRpc]
        private void RpcPlayExplosionEffect(Vector3 position)
        {
            if (isServer)
                return;

            SpawnExplosionEffectLocal(position);
        }
        
        private void SpawnExplosionEffectLocal(Vector3 position)
        {
            if (!_explosionEffectPrefab)
            {
                Debug.LogWarning("[GrenadeProjectile] Explosion effect prefab is not assigned.", this);
                return;
            }

            GameObject effect = Instantiate(
                _explosionEffectPrefab,
                position + Vector3.up * 0.1f,
                Quaternion.identity);

            ParticleSystem[] particleSystems = effect.GetComponentsInChildren<ParticleSystem>(true);
            foreach (ParticleSystem particleSystem in particleSystems)
            {
                particleSystem.Play(true);
            }

            Destroy(effect, 3f);
        }
        
        private int CalculateDamage(float distance)
        {
            if (distance >= _explosionRadius)
                return 0;

            float normalized = 1f - distance / _explosionRadius;
            return Mathf.RoundToInt(_maxDamage * normalized);
        }
    }
}