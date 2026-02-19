using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using UnityEngine;

namespace _Scripts.Player
{
    public sealed class PlayerController : MonoBehaviour
    {
        private IInputService _input;
        private IMovementService _movement;
        private ICameraDirectionProvider _cameraDir;
        private ILoggerService _logger;
        private bool _isInitialized;
        
        public void Init(IInputService input, IMovementService movement, ICameraDirectionProvider cameraDir, 
            ILoggerService logger)
        {
            _logger = logger;
            if (_isInitialized)
            {
                _logger.Warn("Player controller already initialized");
                return;
            }
            _input = input;
            _movement = movement;
            _cameraDir = cameraDir;
            
            _isInitialized = true;
        }
    
        private void FixedUpdate()
        {
            if (!_isInitialized) return;
            var axis = _input.MoveAxis;
            var dir = _cameraDir.Forward * axis.y + _cameraDir.Right * axis.x;
            _movement.Move(dir, Time.fixedDeltaTime);
        }

    }
}