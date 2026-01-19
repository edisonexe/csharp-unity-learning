using Spawning;
using Events;

namespace Services
{
    public class SpawnService
    {
        private readonly ISpawner _spawner;
        private readonly float _interval;
        private readonly GameEventType _spawnEventType;
        private float _timer;
        private bool _enabled;
        
        public SpawnService(ISpawner spawner, float intervalSec, GameEventType spawnEventType)
        {
            _spawner = spawner;
            _interval = intervalSec;
            _spawnEventType = spawnEventType;
        }
        
        public void Enable()
        {
            if (_enabled) return;
            _enabled = true;
            EventBus.Subscribe(HandleGameEvent);
        }

        public void Disable()
        {
            if (!_enabled) return;
            _enabled = false;
            EventBus.Unsubscribe(HandleGameEvent);
        }

        public void Update(float deltaTime)
        {
            if (!_enabled) return;
            
            _timer += deltaTime;
            if (_timer < _interval) return;
            _timer = 0f;
            Spawn();
        }

        private void Spawn()
        {
            _spawner.SpawnOne();
            EventBus.Raise(_spawnEventType, 1);
        }
        
        private void HandleGameEvent(GameEventType type, int value)
        {
            if (type == GameEventType.Win)
                Disable();
        }

    }
}