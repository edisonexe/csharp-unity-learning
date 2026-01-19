using UnityEngine;

namespace Actors.Enemy.AI
{
    public sealed class ChaseState : IEnemyState
    {
        private readonly Transform _player;
        private readonly float _speed;

        public ChaseState(Transform player, float speed)
        {
            _player = player;
            _speed = speed;
        }

        public void Update(EnemyView enemy, float dt)
        {
            if (_player == null) return;

            Vector3 dir = _player.position - enemy.transform.position;
            dir.y = 0f;
            if (dir.sqrMagnitude < 0.0001f) return;

            enemy.transform.position += dir.normalized * (_speed * dt);
        }
    }    
}
