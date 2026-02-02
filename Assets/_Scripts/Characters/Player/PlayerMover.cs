using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player
{
    public sealed class PlayerMover
    {
        private readonly NavMeshAgent _agent;

        public bool HasPath => _agent.hasPath && _agent.remainingDistance > _agent.stoppingDistance;

        public PlayerMover(NavMeshAgent agent)
        {
            _agent = agent;
        }

        public void SetMoveSpeed(float speed)
        {
            if (!_agent) return;
            _agent.speed = Mathf.Max(0f, speed);
        }


        public void MoveTo(Vector3 worldPoint)
        {
            _agent.isStopped = false;
            _agent.SetDestination(worldPoint);
        }

        public void Stop()
        {
            _agent.isStopped = true;
            _agent.ResetPath();
        }
    }
}