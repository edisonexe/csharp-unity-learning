using UnityEngine;

namespace Actors.Enemy.AI
{
    public sealed class ChaseState : IEnemyState
    {
        private readonly Transform _player;
        private readonly float _speed;
        private readonly float _stoppingDistance;
        
        public ChaseState(Transform player, float speed, float stoppingDistance)
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

            enemy.transform.position += dir.normalized * (_speed * dt);
        }
    }    
}
