using System.Collections.Generic;
using Actors.Enemy.AI;
using Events;
using UnityEngine;

namespace Actors.Enemy
{
    public sealed class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private float _stoppingDistance = 2f;
        [SerializeField] private int _damage = 1;
        [SerializeField][Min(0.1f)] private float _damageInterval = 1f;

        private Rigidbody _rb;
        
        private bool _isTouchingPlayer;
        private float _damageTimer;
        
        private Transform _player;
        private IReadOnlyList<Transform> _patrolPoints;
        private IEnemyStrategy _strategy;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (!_rb) Debug.LogError("[EnemyView] Rigidbody - null");
            _strategy = CreateStrategy(_enemyConfig != null ? _enemyConfig.StrategyType : EnemyStrategyType.Patrol);
        }

        public void SetPlayer(Transform player)
        {
            _player = player;
            SetStrategy();
        }

        public void SetPatrolPoints(List<Transform> points)
        {
            _patrolPoints = points;
            SetStrategy();
        }

        private void Update()
        {
            _strategy?.Update(this, Time.deltaTime);
            HandleContactDamage(Time.deltaTime);
        }

        private void HandleContactDamage(float deltaTime)
        {
            if (!_isTouchingPlayer) return;
            _damageTimer += deltaTime;

            if (!(_damageTimer >= _damageInterval)) return;
            _damageTimer = 0f;
            EventBus.Raise(GameEventType.PlayerDamaged, _damage);
        }
        
        private void OnCollisionEnter(Collision other)
        {
            if (!other.collider.CompareTag("Player")) return;

            _isTouchingPlayer = true;
            _damageTimer = 0f;
            
            EventBus.Raise(GameEventType.PlayerDamaged, _damage);
        }

        private void OnCollisionExit(Collision other)
        {
            if (!other.collider.CompareTag("Player")) return;

            _isTouchingPlayer = false;
            _damageTimer = 0f;
        }

        public void MoveBy(Vector3 delta)
        {
            if (_rb != null)
                _rb.MovePosition(_rb.position + delta);
            else
                transform.position += delta;
        }

        private void SetStrategy()
        {
            if (_strategy == null) return;

            var speed = _enemyConfig ? _enemyConfig.EnemyMoveSpeed : 1.5f;
            _strategy.Init(_player, _patrolPoints, speed, _stoppingDistance);
        }
        
        private static IEnemyStrategy CreateStrategy(EnemyStrategyType type)
        {
            return type switch
            {
                EnemyStrategyType.Chase => new ChaseStrategy(),
                _ => new PatrolStrategy(),
            };
        }
    }   
}