using UnityEngine;
using UnityEngine.InputSystem;

namespace Camera
{
    public class CameraRig : MonoBehaviour
    {
        [SerializeField] private Transform _target;
        [SerializeField] private float _rotationSpeed = 5f;
        [SerializeField] private bool _rotateWhileRMB = true;

        private float _rotationY;

        private void LateUpdate()
        {
            if (!_target || Mouse.current == null)
            {
                Debug.LogError("[CameraRig] Target object or Mouse is null.");
            }

            transform.position = _target.position;
            bool allowRotate = !_rotateWhileRMB || Mouse.current.rightButton.isPressed;
            if (allowRotate)
            {
                float mouseX = Mouse.current.delta.ReadValue().x;
                _rotationY += mouseX * _rotationSpeed * Time.deltaTime;
            }

            transform.rotation = Quaternion.Euler(0f, _rotationY, 0f);
        }
    }
}
