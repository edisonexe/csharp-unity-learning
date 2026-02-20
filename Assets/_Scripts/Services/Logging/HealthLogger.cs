using _Scripts.Interfaces;

namespace _Scripts.Services.Logging
{
    public sealed class HealthLogger
    {
        public HealthLogger(IHealth health, ILoggerService logger)
        {
            health.Changed += (cur, max) => logger.Log($"Health changed: {cur}/{max}");
            health.Died += () => logger.Log("Player died");
        }
    }

}