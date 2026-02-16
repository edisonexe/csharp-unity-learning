using _Scripts.Interfaces;

namespace _Scripts.Services.Logging
{
    public sealed class HealthLogger
    {
        public HealthLogger(IHealth health, ILoggerService logger)
        {
            health.Damaged += dmg => logger.Log($"Damage: -{dmg}, Current health: {health.Current}");
            health.Died += () => logger.Log("Player died");
        }
    }

}