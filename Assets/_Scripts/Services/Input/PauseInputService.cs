using _Scripts.Interfaces;

namespace _Scripts.Services.Input
{
    public class PauseInputService : IPauseInputService
    {
        private readonly GameInputActions _inputActions;
        
        private ILoggerService _logger;
        
        public PauseInputService(GameInputActions inputActions, ILoggerService logger)
        {
            _inputActions = inputActions;
            _logger = logger;
        }
        
        public bool PausePressedThisFrame => _inputActions.Player.Pause.WasPressedThisFrame();
    }
}