using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public sealed class PlayerView : MonoBehaviour
    {
        private Rigidbody _rb;
        
        private Vector2 _moveInput;
        public Vector2 MoveInput => _moveInput;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody>();
            if (!_rb) Debug.LogError("[PlayerView] Rigidbody - null");
        }

        public void OnMove(InputAction.CallbackContext ctx) => _moveInput = ctx.ReadValue<Vector2>();

        public void ApplyMove(Vector3 delta) => _rb.MovePosition(_rb.position + delta);
    }
}