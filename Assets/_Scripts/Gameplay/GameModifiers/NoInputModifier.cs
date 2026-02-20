using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;

namespace _Scripts.Gameplay.GameModifiers
{
    public sealed class NoInputModifier : IGameModifier
    {
        private readonly IInputToggle _inputToggle;
        private readonly float _period;
        private readonly float _duration;
        private readonly ILoggerService _logger;

        private float _timer;
        private float _disabledTimer;
        private bool _isDisabled;

        public NoInputModifier(IInputToggle inputToggle, float period, float duration, ILoggerService logger)
        {
            _inputToggle = inputToggle;
            _period = period;
            _duration = duration;
            _logger = logger;
        }

        public void OnEnterGameplay()
        {
            _logger.Log("[NoInputModifier]: Activated");
            _timer = 0f;
            _disabledTimer = 0f;
            _isDisabled = false;
        }

        public void OnExitGameplay()
        {
            _logger.Log("[NoInputModifier]: Deactivated");
            _inputToggle?.Enable();
            _isDisabled = false;
        }

        public void Tick(float deltaTime)
        {
            if (_inputToggle == null) return;

            if (_isDisabled)
            {
                _disabledTimer += deltaTime;
                if (_disabledTimer >= _duration)
                {
                    _disabledTimer = 0f;
                    _isDisabled = false;
                    _inputToggle.Enable();
                }
                return;
            }

            _timer += deltaTime;
            if (_timer >= _period)
            {
                _timer -= _period;
                _isDisabled = true;
                _inputToggle.Disable();
            }
        }
    }
}