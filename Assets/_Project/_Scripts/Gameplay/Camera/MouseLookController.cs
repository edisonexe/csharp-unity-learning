using _Project._Scripts.Interfaces;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Camera
{
    public sealed class MouseLookController : MonoBehaviour, ILookController
    {
        [SerializeField] private Transform _yawRoot;
        [SerializeField] private Transform _pitchRoot;
        [SerializeField] private float _sensitivity = 2.5f;
        [SerializeField] private float _minPitch = -80f;
        [SerializeField] private float _maxPitch = 80f;

        private float _pitch;

        public float Pitch => _pitch;
        public float Yaw => _yawRoot ? _yawRoot.eulerAngles.y : 0f;

        public void Apply(float yawDelta, float pitchDelta)
        {
            if (!_yawRoot || !_pitchRoot)
                return;

            _yawRoot.Rotate(Vector3.up * (yawDelta * _sensitivity));

            _pitch -= pitchDelta * _sensitivity;
            _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

            _pitchRoot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }

        public void SetYaw(float yaw)
        {
            if (!_yawRoot)
                return;

            Vector3 euler = _yawRoot.eulerAngles;
            euler.y = yaw;
            _yawRoot.eulerAngles = euler;
        }

        public void SetPitch(float pitch)
        {
            if (!_pitchRoot)
                return;

            _pitch = Mathf.Clamp(pitch, _minPitch, _maxPitch);
            _pitchRoot.localRotation = Quaternion.Euler(_pitch, 0f, 0f);
        }
    }
}