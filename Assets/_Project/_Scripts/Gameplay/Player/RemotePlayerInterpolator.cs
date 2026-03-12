using _Project._Scripts.Interfaces;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Player
{
    public sealed class RemotePlayerInterpolator : MonoBehaviour, IPlayerStateBroadcaster
    {
        [SerializeField] private Transform _positionRoot;
        [SerializeField] private Transform _pitchRoot;

        [Header("Position")]
        [SerializeField] private float _positionLerpSpeed = 12f;
        [SerializeField] private float _snapDistance = 3f;

        [Header("Rotation")]
        [SerializeField] private float _yawDegreesPerSecond = 540f;
        [SerializeField] private float _pitchDegreesPerSecond = 540f;

        private Vector3 _targetPosition;
        private float _targetYaw;
        private float _targetPitch;
        private bool _hasTarget;

        public void SetTarget(Vector3 position, float yaw, float pitch)
        {
            _targetPosition = position;
            _targetYaw = yaw;
            _targetPitch = pitch;

            if (_hasTarget)
                return;

            if (_positionRoot)
            {
                _positionRoot.position = position;
                _positionRoot.rotation = Quaternion.Euler(0f, yaw, 0f);
            }

            if (_pitchRoot)
                _pitchRoot.localRotation = Quaternion.Euler(pitch, 0f, 0f);

            _hasTarget = true;
        }

        private void Update()
        {
            if (!_hasTarget || !_positionRoot)
                return;

            float deltaTime = Time.deltaTime;

            float distance = Vector3.Distance(_positionRoot.position, _targetPosition);
            if (distance > _snapDistance)
            {
                _positionRoot.position = _targetPosition;
            }
            else
            {
                _positionRoot.position = Vector3.Lerp(
                    _positionRoot.position,
                    _targetPosition,
                    deltaTime * _positionLerpSpeed);
            }

            Quaternion targetYawRotation = Quaternion.Euler(0f, _targetYaw, 0f);
            _positionRoot.rotation = Quaternion.RotateTowards(
                _positionRoot.rotation,
                targetYawRotation,
                _yawDegreesPerSecond * deltaTime);

            if (_pitchRoot)
            {
                Quaternion targetPitchRotation = Quaternion.Euler(_targetPitch, 0f, 0f);
                _pitchRoot.localRotation = Quaternion.RotateTowards(
                    _pitchRoot.localRotation,
                    targetPitchRotation,
                    _pitchDegreesPerSecond * deltaTime);
            }
        }
    }
}