using System;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.Entities
{
    [AddComponentMenu("StressTest/Entities/Target")]
    public class Target : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 5f;
        
        private Vector3 _moveDirection;
        private Action<Target> _onDespawn;

        private const string PROJ_TAG = "Projectile";
        private const string SAFEZONE_TAG = "SafeZone";
        
        public float Speed => _speed;
        public Vector3 MoveDirection => _moveDirection;
        
        public void Init(Vector3 position, Vector3 moveDirection, Action<Target> onDespawn)
        {
            transform.position = position;
            _moveDirection = moveDirection;
            _onDespawn = onDespawn;
        }
        
        public void OnSpawn() { }
        public void OnDespawn() { }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PROJ_TAG)) Despawn();
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(SAFEZONE_TAG)) Despawn();
        }

        private void Despawn() => _onDespawn?.Invoke(this);
    }
}