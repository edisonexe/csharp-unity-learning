using System;
using Characters.Player;
using Configs;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Renderer))]
    public sealed class EnemyController : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _cfg;
        public event Action<EnemyController> Died;
        
        private NavMeshAgent _agent;
        private Renderer _renderer;

        private EnemyEntity _entity;

        private PlayerEntity _playerEntity;
        private Transform _playerTransform;

        private bool _enabled;
        
        public void ConstructEnemy(PlayerEntity playerEntity, Transform playerTransform)
        {
            _playerEntity = playerEntity;
            _playerTransform = playerTransform;
            _playerEntity.Died += OnPlayerDied;
            Enable();
        }
        
        private void Awake()
        {
            _entity = new EnemyEntity(_cfg.MaxHp, _cfg.MoveSpeed, _cfg.Damage, _cfg.AttackCooldown);
            
            _agent = GetComponent<NavMeshAgent>();
            _renderer = GetComponent<Renderer>();
            
            _agent.speed = _cfg.MoveSpeed;
            _agent.stoppingDistance = _cfg.AttackDistance;
            
            if (_renderer != null && _cfg.EnemyMaterial != null)
            {
                _renderer.material = _cfg.EnemyMaterial;
            }
        }

        private void Update()
        {
            if (!_enabled) return;
            
            if (!_playerTransform || _entity.IsDead)
                return;

            HandleMovement();
            TryAttack();
        }

        private void OnDestroy()
        {
            if (_playerEntity != null) _playerEntity.Died -= OnPlayerDied;
            Disable();
        }

        public void TakeDamage(int damage)
        {
            _entity.TakeDamage(damage);

            if (_entity.IsDead)
            {
                Died?.Invoke(this);
                Destroy(gameObject);
            }
        }
        
        private void Enable()
        {
            if (_enabled) return;
            _enabled = true;
        }

        private void Disable()
        {
            if (!_enabled) return;
            _enabled = false;
        }
        
        private void HandleMovement()
        {
            if (_agent.isOnNavMesh)
            {
                _agent.SetDestination(_playerTransform.position);
            }
        }

        private void TryAttack()
        {
            var sqrDistance = (_playerTransform.position - transform.position).sqrMagnitude;

            var attackDistance = _cfg.AttackDistance;
            if (sqrDistance > attackDistance * attackDistance)
                return;
            
            var time = Time.time;
            if (!_entity.CanAttack(time)) return;

            _playerEntity.TakeDamage(_entity.Damage);
            _entity.MarkAttack(time);
        }

        private void OnPlayerDied()
        {
            _enabled = false;
            if (_agent) _agent.isStopped = true;
        }
    }
}
