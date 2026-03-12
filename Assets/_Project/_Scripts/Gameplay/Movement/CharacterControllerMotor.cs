using _Project._Scripts.Interfaces;
using UnityEngine;

namespace _Project._Scripts.Gameplay.Movement
{
    [RequireComponent(typeof(CharacterController))]
    public sealed class CharacterControllerMotor : MonoBehaviour, IMotor
    {
        [SerializeField] private float _moveSpeed = 5f;
        [SerializeField] private float _jumpHeight = 1.2f;
        [SerializeField] private float _gravity = -20f;

        private CharacterController _characterController;
        private float _verticalVelocity;

        public Vector3 Position => transform.position;
        public bool IsGrounded => _characterController && _characterController.isGrounded;

        public void Construct(CharacterController characterController)
        {
            _characterController = characterController;
        }

        public void Simulate(Vector2 moveInput, bool jumpPressed, float deltaTime)
        {
            if (!_characterController)
                return;

            if (_characterController.isGrounded && _verticalVelocity < 0f)
                _verticalVelocity = -2f;

            Vector3 move = transform.right * moveInput.x + transform.forward * moveInput.y;

            Vector3 horizontalVelocity = move.normalized * _moveSpeed;

            if (jumpPressed && _characterController.isGrounded)
                _verticalVelocity = Mathf.Sqrt(_jumpHeight * -2f * _gravity);

            _verticalVelocity += _gravity * deltaTime;

            Vector3 velocity = new Vector3(horizontalVelocity.x, _verticalVelocity, horizontalVelocity.z);
            _characterController.Move(velocity * deltaTime);
        }

        public void Teleport(Vector3 position)
        {
            if (!_characterController)
                return;

            _characterController.enabled = false;
            transform.position = position;
            _characterController.enabled = true;
        }
    }
}