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
        private HashSet<Target> _activeTargets;
        private Target _currentTarget;
        private float _fireTimer;
        private Quaternion _lookRotation;
        private Transform _yawTransform;

        public void Init(EntityFactory factory, HashSet<Target> activeTargets)
        {
            _factory = factory ?? throw new System.ArgumentNullException(nameof(factory));
            _activeTargets = activeTargets ?? throw new System.ArgumentNullException(nameof(activeTargets));
            
            _yawTransform = _yawRoot;
            _lookRotation = _yawTransform.rotation;

            if (!_yawRoot) Debug.LogError("[TurretController] YawRoot is null!", this);
        }

        private void Update()
        {
            Vector3 gunPos = _yawTransform.position;
            Vector3 gunForward = _yawTransform.forward;

            if (!IsTargetValid(_currentTarget, gunPos, gunForward))
                _currentTarget = FindBestTarget(gunPos, gunForward);

            if (_currentTarget)
            {
                UpdateTargetRotation(_currentTarget.transform.position, gunPos);

                if (Quaternion.Angle(_yawTransform.rotation, _lookRotation) < 15f)
                    HandleFireTiming(gunForward);
            }

            _yawTransform.rotation = Quaternion.RotateTowards(
                _yawTransform.rotation,
                _lookRotation,
                _rotationSpeed * Time.deltaTime
            );
        }

        private bool IsTargetValid(Target target, Vector3 myPos, Vector3 myForward)
        {
            if (!target || !target.gameObject.activeSelf) return false;

            Vector3 dirToTarget = (target.transform.position - myPos).normalized;
            return Vector3.Angle(myForward, dirToTarget) < _fovAngle * 0.5f;
        }

        private void UpdateTargetRotation(Vector3 targetPos, Vector3 myPos)
        {
            Vector3 direction = (targetPos - myPos).normalized;
            direction.y = 0;
            if (direction != Vector3.zero) _lookRotation = Quaternion.LookRotation(direction);
        }

        private void HandleFireTiming(Vector3 shootDirection)
        {
            _fireTimer += Time.deltaTime;
            if (_fireTimer >= _fireRate)
            {
                _fireTimer = 0;
                _factory.CreateProjectile(_shootingPoint.position, _yawTransform.rotation, shootDirection);
            }
        }

        private Target FindBestTarget(Vector3 myPos, Vector3 myForward)
        {
            Target best = null;
            float minScore = float.MaxValue;
            float halfFov = _fovAngle * 0.5f;
            
            foreach (var t in _activeTargets)
            {
                if (!t || !t.gameObject.activeSelf) continue;

                Vector3 diff = t.transform.position - myPos;
                float sqrDist = diff.sqrMagnitude;

                Vector3 dirToTarget = diff.normalized;
                float angle = Vector3.Angle(myForward, dirToTarget);

                if (angle > halfFov) continue;

                float score = sqrDist + angle;

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