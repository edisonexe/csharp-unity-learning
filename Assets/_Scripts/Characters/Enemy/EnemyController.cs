using System;
using Characters.Player;
using Configs;
using Pooling;
using UnityEngine;
using UnityEngine.AI;

namespace Characters.Enemy
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Renderer))]
    public sealed class EnemyController : MonoBehaviour, IPoolable
    {
        [SerializeField] private EnemyConfig _cfg;
        [SerializeField][Min(0.1f)] private float _repathInterval = 0.2f;
        [SerializeField][Min(0.5f)] private float _repathDistance = 0.75f;

        private float _nextRepathTime;
        private Vector3 _lastTargetPos;

        private ObjectPool<EnemyController> _pool;
        
        private NavMeshAgent _agent;
        private NavMeshPath _path;
        
        private MeshRenderer _renderer;
        private MaterialPropertyBlock _mpb;
        private static readonly int BaseColorID = Shader.PropertyToID("_BaseColor");
        
        private EnemyEntity _entity;

        private PlayerEntity _playerEntity;
        private Transform _playerTransform;
        
        private Action<EnemyController> _onDied;
        public void SetOnDied(Action<EnemyController> onDied) => _onDied = onDied;
        
        private bool _enabled;

        public void ConstructEnemy(PlayerEntity playerEntity, Transform playerTransform)
        {
            _playerEntity = playerEntity;
            _playerTransform = playerTransform;
        }
        
        private void Awake()
        {
            _entity = new EnemyEntity(_cfg.MaxHp, _cfg.MoveSpeed, _cfg.Damage, _cfg.AttackCooldown);
            
            _agent = GetComponent<NavMeshAgent>();
            _path = new NavMeshPath();
            
            _renderer = GetComponent<MeshRenderer>();
            _mpb =  new MaterialPropertyBlock();
            
            _agent.speed = _cfg.MoveSpeed;
            _agent.stoppingDistance = _cfg.AttackDistance;
            
            ApplyColor(_cfg.Color);
        }

        private void Update()
        {
            if (!_enabled) return;
            if (!_playerTransform || _entity.IsDead) return;

            var attackDist = _cfg.AttackDistance;
            var sqrDist = (_playerTransform.position - transform.position).sqrMagnitude;
            if (sqrDist <= attackDist * attackDist)
            {
                if (_agent && !_agent.isStopped) _agent.isStopped = true;
                TryAttack();
                return;
            }

            HandleMovement();
        }
        
        public void TakeDamage(int damage)
        {
            _entity.TakeDamage(damage);
            if (_entity.IsDead)
            {
                _onDied?.Invoke(this);
                _pool.Return(this);
            }
        }
        
        public void BindPool(ObjectPool<EnemyController> pool) => _pool = pool;
        
        private void HandleMovement()
        {
            if (!_agent.isOnNavMesh) return;

            var target = _playerTransform.position;
            
            if (Time.time < _nextRepathTime && (target - _lastTargetPos).sqrMagnitude < _repathDistance * _repathDistance)
                return;

            // _nextRepathTime = Time.time + _repathInterval;
            _nextRepathTime = Time.time + UnityEngine.Random.value * _repathInterval;
            _lastTargetPos = target;

            bool hasPath = _agent.CalculatePath(target, _path) && _path.status == NavMeshPathStatus.PathComplete;

            if (!hasPath)
            {
                
                if (!_agent.isStopped) _agent.isStopped = true;
                
                _agent.ResetPath();
                _agent.velocity = Vector3.zero;
                return;
            }
            
            _agent.isStopped = false;
            _agent.SetDestination(target);
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
        
        public void OnSpawned()
        {
            _entity.ResetState();
            _enabled = true;

            if (_agent)
            {
                _agent.isStopped = false;
                _agent.ResetPath();
                _agent.velocity = Vector3.zero;
            }

            _nextRepathTime = 0f;
            _lastTargetPos = transform.position;
        }

        public void OnDespawned()
        {
            _enabled = false;

            if (_agent)
            {
                _agent.isStopped = true;
                _agent.ResetPath();
                _agent.velocity = Vector3.zero;
            }
            _onDied = null; 

            _playerEntity = null;
            _playerTransform = null;
        }
        
        public void ForceDespawn() => _pool?.Return(this);
        
        private void ApplyColor(Color color)
        {
            if (!_renderer) return;

            _renderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(BaseColorID, color);
            _renderer.SetPropertyBlock(_mpb);
        }
    }
}
