using _Scripts.Interfaces.Input;
using UnityEngine;

namespace _Scripts.Camera
{
    public sealed class CameraRigController : MonoBehaviour
    {
        [Header("Camera Target")]
        [SerializeField] private Transform _target;

        [Header("Rotation settings")]
        [SerializeField][Min(0.1f)] private float _yawSpeed = 0.7f;
        [SerializeField][Min(0.1f)] private float _pitchSpeed = 0.5f;
        [SerializeField] private float _minPitch = -30f;
        [SerializeField] private float _maxPitch = 60f;

        private ILookInputService _lookInput;

        private float _yaw;
        private float _pitch;

        public void Init(ILookInputService lookInput, Transform target)
        {
            _lookInput = lookInput;
            _target = target;
            
            var e = transform.eulerAngles;
            _yaw = e.y;
            _pitch = NormalizePitch(e.x);
        }

        private void LateUpdate()
        {
            if (!_target)
                return;
            
            transform.position = _target.position;

            if (_lookInput == null) return;

            if (_lookInput.RotateHeld)
            {
                var d = _lookInput.LookDelta;
                _yaw += d.x * _yawSpeed;
                _pitch -= d.y * _pitchSpeed;
                _pitch = Mathf.Clamp(_pitch, _minPitch, _maxPitch);

                transform.rotation = Quaternion.Euler(_pitch, _yaw, 0f);
            }
        }

        private static float NormalizePitch(float xEuler)
        {
            if (xEuler > 180f) xEuler -= 360f;
            return xEuler;
        }
    }

}