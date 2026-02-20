using _Scripts.Interfaces;
using _Scripts.Interfaces.Spawn;

namespace _Scripts.Gameplay.GameModifiers
{
    public sealed class FastSpawnModifier : IGameModifier
    {
        private readonly ISpawnRate _spawnRate;
        private readonly float _multiplier;
        private readonly ILoggerService _logger;

        private float _prevMultiplier;

        public FastSpawnModifier(ISpawnRate spawnRate, float multiplier, ILoggerService logger)
        {
            _spawnRate = spawnRate;
            _multiplier = multiplier;
            _logger = logger;
        }

        public void OnEnterGameplay()
        {
            if (_spawnRate == null)
            {
                _logger?.Warn("FastSpawnModifier: spawnRate is null");
                return;
            }
            
            _logger.Log("[FastSpawnModifier]: Activated");
            _prevMultiplier = _spawnRate.IntervalMultiplier;
            _spawnRate.IntervalMultiplier = _multiplier;
        }

        public void OnExitGameplay()
        {
            if (_spawnRate == null) return;
            _logger.Log("[FastSpawnModifier]: Deactivated");
            _spawnRate.IntervalMultiplier = _prevMultiplier;
        }

        public void Tick(float deltaTime) { }
    }
}