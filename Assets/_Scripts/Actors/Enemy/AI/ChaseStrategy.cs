using System.Collections.Generic;
using UnityEngine;

namespace Actors.Enemy.AI
{
    public sealed class ChaseStrategy : IEnemyStrategy
    {
        private Transform _player;
        private float _speed;
        private float _stoppingDistance;

        public void Init(Transform player, IReadOnlyList<Transform> patrolPoints, float speed, float stoppingDistance)
        {
            _player = player;
            _speed = speed;
            _stoppingDistance = stoppingDistance;
        }

        public void Update(EnemyView enemy, float dt)
        {
            if (!_player) return;

            Vector3 dir = _player.position - enemy.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < _stoppingDistance) return;

            enemy.MoveBy(dir.normalized * (_speed * dt));
        }
    }    
}
