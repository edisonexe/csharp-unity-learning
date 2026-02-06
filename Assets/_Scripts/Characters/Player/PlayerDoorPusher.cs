using UnityEngine;
using UnityEngine.AI;

namespace Characters.Player
{
    [RequireComponent(typeof(NavMeshAgent))]
    public class PlayerDoorPusher : MonoBehaviour
    {
        [SerializeField] private float _pushImpulse = 2.5f;
        private NavMeshAgent _agent;

        private void Awake()
        {
            if (!_agent) _agent = GetComponent<NavMeshAgent>();
        }

        private void OnCollisionStay(Collision collision)
        {
            var rb = collision.rigidbody;
            if (rb == null) return;
        
            Vector3 dir = _agent != null ? _agent.desiredVelocity : Vector3.zero;
            dir.y = 0f;
        
            if (dir.sqrMagnitude < 0.0001f)
                dir = transform.forward;

            dir.Normalize();
        
            Vector3 p = collision.GetContact(0).point;
            rb.AddForceAtPosition(dir * _pushImpulse, p, ForceMode.Impulse);
        }
    }
}
