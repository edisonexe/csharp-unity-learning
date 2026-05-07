using System;
using StressTest.Interfaces;
using UnityEngine;

namespace StressTest.Gameplay
{
    public class Projectile : MonoBehaviour, IPoolable
    {
        [SerializeField] private float _speed = 20f;
        [SerializeField] private float _maxLifetime = 3f;
        
        private float _currentLifetime;
        private Vector3 _direction;
        private bool _isPoolMode;
        private Action<Projectile> _onDespawn;
        
        private const string TARGET_TAG = "Target";
        
        public void Init(Vector3 position, Quaternion rotation, Vector3 dir, 
                            bool isPoolMode, System.Action<Projectile> onDespawn)
        {
            transform.position = position;
            transform.rotation = rotation;
            _direction = dir;
            _isPoolMode = isPoolMode;
            _onDespawn = onDespawn;
        }
        
        public void OnSpawn()
        {
            _currentLifetime = 0;
        }
        public void OnDespawn() { }

        private void Update()
        {
            transform.Translate(_direction * (_speed * Time.deltaTime), Space.World);
            _currentLifetime += Time.deltaTime;

            if (_currentLifetime >= _maxLifetime)
            {
                Despawn();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag(TARGET_TAG))
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