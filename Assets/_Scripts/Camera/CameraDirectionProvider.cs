using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Camera
{
    public sealed class CameraDirectionProvider : ICameraDirectionProvider
    {
        private readonly Transform _cameraTransform;

        public CameraDirectionProvider(Transform cameraTransform)
        {
            _cameraTransform = cameraTransform;
        }

        public Vector3 Forward
        {
            get
            {
                var f = _cameraTransform.forward;
                f.y = 0f;
                return f.normalized;
            }
        }

        public Vector3 Right
        {
            get
            {
                var r = _cameraTransform.right;
                r.y = 0f;
                return r.normalized;
            }
        }
    }
}