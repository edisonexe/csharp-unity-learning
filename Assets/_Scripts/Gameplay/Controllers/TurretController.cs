using System.Collections.Generic;
using _Scripts.Gameplay.Entities;
using _Scripts.Gameplay.Services;
using UnityEngine;

namespace _Scripts.Gameplay.Controllers
{
    [AddComponentMenu("StressTest/Controllers/Turret Controller")]
    public class TurretController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private Transform _yawRoot;
        [SerializeField] private Transform _shootingPoint;

        [Header("Settings")]
        [SerializeField] private float _fireRate = 0.01f;
        [SerializeField] private float _rotationSpeed = 270f; 
        [SerializeField] private float _fovAngle = 120f;

        private EntityFactory _factory;
        private List<Target> _activeTargets;
        private Target _currentTarget;
        private float _fireTimer;
        private Quaternion _lookRotation;

        public void Init(EntityFactory factory, List<Target> activeTargets)
        {
            _factory = factory;
            _activeTargets = activeTargets;
            _lookRotation = _yawRoot.rotation;
        }

        private void Update()
        {
            if (!IsTargetValid(_currentTarget)) _currentTarget = FindBestTarget();

            if (_currentTarget)
            {
                UpdateTargetRotation(_currentTarget.transform.position);
                
                if (Quaternion.Angle(_yawRoot.rotation, _lookRotation) < 15f) HandleFireTiming();
            }

            _yawRoot.rotation = Quaternion.RotateTowards(
                _yawRoot.rotation, 
                _lookRotation, 
                _rotationSpeed * Time.deltaTime
            );
        }

        private bool IsTargetValid(Target target)
        {
            if (!target || !target.gameObject.activeSelf) return false;
            
            Vector3 dirToTarget = (target.transform.position - _yawRoot.position).normalized;
            float angle = Vector3.Angle(transform.forward, dirToTarget); 
            
            return angle < _fovAngle * 0.5f;
        }

        private void UpdateTargetRotation(Vector3 targetPos)
        {
            Vector3 direction = (targetPos - _yawRoot.position).normalized;
            direction.y = 0;
            if (direction != Vector3.zero) _lookRotation = Quaternion.LookRotation(direction);
        }

        private void HandleFireTiming()
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireRate)
            {
                _fireTimer = 0;
                _factory.CreateProjectile(_shootingPoint.position, _yawRoot.rotation, _yawRoot.forward);
            }
        }

        private Target FindBestTarget()
        {
            Target best = null;
            float minScore = float.MaxValue;

            for (int i = 0; i < _activeTargets.Count; i++)
            {
                Target t = _activeTargets[i];
                if (!t || !t.gameObject.activeSelf) continue;

                Vector3 dirToTarget = (t.transform.position - _yawRoot.position).normalized;
                float angle = Vector3.Angle(transform.forward, dirToTarget);
                
                if (angle > _fovAngle * 0.5f) continue;
                
                float dist = Vector3.Distance(transform.position, t.transform.position);
                float score = dist + angle; 

                if (score < minScore)
                {
                    minScore = score;
                    best = t;
                }
            }
            return best;
        }
    }
}