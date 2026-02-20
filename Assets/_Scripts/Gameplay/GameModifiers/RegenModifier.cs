using _Scripts.Interfaces;

namespace _Scripts.Gameplay.GameModifiers
{
    public sealed class RegenModifier : IGameModifier
    {
        private readonly IHealth _health;
        private readonly float _interval;
        private readonly int _amount;
        private readonly ILoggerService _logger;
        
        private float _timer;
        
        public RegenModifier(IHealth health, float interval, int amount, ILoggerService logger)
        {
            _health = health;
            _interval = interval;
            _amount = amount;
            _logger = logger;
        }
        
        public void OnEnterGameplay()
        {
            _logger.Log("[RegenModifier]: Activated");
            _timer = 0;
        }

        public void OnExitGameplay()
        {
            _logger.Log("[RegenModifier]: Deactivated");
        }

        public void Tick(float deltaTime)
        {
            _timer += deltaTime;

            while (_timer >= _interval)
            {
                _timer -= _interval;

                if (_health.Current >= _health.Max)
                    return;

                _health.Heal(_amount);
                
                _logger?.Log($"Regen health +{_amount}");
            }
        }
    }
}