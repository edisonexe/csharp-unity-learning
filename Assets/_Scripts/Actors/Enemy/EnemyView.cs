using System.Collections.Generic;
using Actors.Enemy.AI;
using UnityEngine;

namespace Actors.Enemy
{
    public sealed class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyConfig _enemyConfig;
        [SerializeField] private float _chaseDistance = 12f;
        [SerializeField] private float _stoppingDistance = 2f;
        
        private Transform _player;

        private IEnemyState _patrolState;
        private IEnemyState _chaseState;
        private IEnemyState _currentState;

        private void Awake() => _currentState = _patrolState;

        public void SetPlayer(Transform player)
        {
            _player = player;
            _chaseState = new ChaseState(_player, _enemyConfig.EnemyMoveSpeed, _stoppingDistance);
        }

        public void SetPatrolPoints(List<Transform> points)
        {
            _patrolState = new PatrolState(points, _enemyConfig.EnemyMoveSpeed);
            if (_currentState == null) _currentState = _patrolState;
        }
        
        private void Update()
        {
            if (_player)
            {
                float distance = Vector3.Distance(transform.position, _player.position);
                _currentState = distance <= _chaseDistance
                    ? _chaseState
                    : _patrolState;
            }

            _currentState?.Update(this, Time.deltaTime);
        }
    }   
}