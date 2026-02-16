using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        private IInputService _input;
        private IMovementService _movement;
        private ICameraDirectionProvider _cameraDir;
        public void Init(IInputService input, IMovementService movement, ICameraDirectionProvider cameraDir)
        {
            _input = input;
            _movement = movement;
            _cameraDir = cameraDir;
        }
    
        private void FixedUpdate()
        {
            var axis = _input.MoveAxis;
            var dir = _cameraDir.Forward * axis.y + _cameraDir.Right * axis.x;
            _movement.Move(dir, Time.fixedDeltaTime);
        }

    }
}