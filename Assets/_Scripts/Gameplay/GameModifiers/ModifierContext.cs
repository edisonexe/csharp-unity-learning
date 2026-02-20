using _Scripts.Interfaces;
using _Scripts.Interfaces.Input;
using _Scripts.Interfaces.Spawn;

namespace _Scripts.Gameplay.GameModifiers
{
    public sealed class ModifierContext
    {
        public readonly ILoggerService Logger;
        public readonly IHealth Health;
        public readonly IInputToggle InputToggle;
        public readonly ISpawnRate SpawnRate;

        public ModifierContext(ILoggerService logger, IHealth health, IInputToggle inputToggle, ISpawnRate spawnRate)
        {
            Logger = logger;
            Health = health;
            InputToggle = inputToggle;
            SpawnRate = spawnRate;
        }
    }
}