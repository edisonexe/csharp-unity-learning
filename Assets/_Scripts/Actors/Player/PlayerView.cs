using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public sealed class PlayerView : MonoBehaviour
    {
        private Vector2 _moveInput;
        public Vector2 MoveInput => _moveInput;
    
        public void OnMove(InputAction.CallbackContext ctx) => _moveInput = ctx.ReadValue<Vector2>();

        public void ApplyMove(Vector3 delta) => transform.position += delta;
    }
}