using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public sealed class EnemyView : MonoBehaviour
    {
        [SerializeField] private EnemyConfig enemyConfig;
        [SerializeField] private float chaseDistance = 6f;

        private Transform _player;

        private IEnemyState _patrolState;
        private IEnemyState _chaseState;
        private IEnemyState _currentState;

        private void Awake()
        {
            _currentState = _patrolState;
        }

        public void SetPlayer(Transform player)
        {
            _player = player;
            _chaseState = new ChaseState(_player, enemyConfig.EnemyMoveSpeed);
        }

        public void SetPatrolPoints(List<Transform> points)
        {
            _patrolState = new PatrolState(points, enemyConfig.EnemyMoveSpeed);
            if (_currentState == null) _currentState = _patrolState;
        }
        
        private void Update()
        {
            if (_player != null)
            {
                float distance = Vector3.Distance(transform.position, _player.position);
                _currentState = distance <= chaseDistance
                    ? _chaseState
                    : _patrolState;
            }

            _currentState?.Update(this, Time.deltaTime);
        }
    }   
}