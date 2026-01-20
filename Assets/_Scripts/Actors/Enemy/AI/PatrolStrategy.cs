using System.Collections.Generic;
using UnityEngine;

namespace Actors.Enemy.AI
{
    public sealed class PatrolStrategy : IEnemyStrategy
    {
        private IReadOnlyList<Transform> _points;
        private float _speed;
        private int _index;

        public void Init(Transform player, IReadOnlyList<Transform> patrolPoints, float speed, float stoppingDistance)
        {
            _points = patrolPoints;
            _speed = speed;
            _index = 0;
        }

        public void Update(EnemyView enemy, float dt)
        {
            if (_points == null || _points.Count == 0) return;

            Transform target = _points[_index];
            Move(enemy, target.position, dt);

            Vector3 toTarget = target.position - enemy.transform.position;
            toTarget.y = 0f;

            if (toTarget.sqrMagnitude < 0.2f * 0.2f)
                _index = (_index + 1) % _points.Count;
        }

        private void Move(EnemyView enemy, Vector3 targetPos, float dt)
        {
            Vector3 dir = targetPos - enemy.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return;

            enemy.MoveBy(dir.normalized * (_speed * dt));
        }
    }   
}