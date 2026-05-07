using System;
using StressTest.Interfaces;
using UnityEngine;

namespace StressTest.Gameplay
{
    public class Target : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 5f;
        
        private Vector3 _moveDirection;
        private bool _isPoolMode;
        private Action<Target> _onDespawn;

        private const string PROJ_TAG = "Projectile";
        private const string SAFEZONE_TAG = "SafeZone";
        
        public void Init(Vector3 position, Vector3 moveDirection, bool isPoolMode, Action<Target> onDespawn)
        {
            transform.position = position;
            _moveDirection = moveDirection;
            _isPoolMode = isPoolMode;
            _onDespawn = onDespawn;
        }
        
        public void OnSpawn() { }
        public void OnDespawn() { }
        
        private void Update()
        {
            transform.Translate(_moveDirection * (_speed * Time.deltaTime), Space.World);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(PROJ_TAG))
            {
                Despawn();
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.CompareTag(SAFEZONE_TAG))
            {
                Despawn();
            }
        }

        private void Despawn()
        {
            _onDespawn?.Invoke(this);
            if (!_isPoolMode)
            {
                Destroy(gameObject);
            }
        }
    }
}