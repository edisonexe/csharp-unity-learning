using Events;
using Spawning;

namespace Services
{
    public class SpawnService
    {
        private readonly IEnemyFactory _factory;
        private readonly float _interval;
        private float _timer;

        public SpawnService(IEnemyFactory factory, float intervalSec)
        {
            _factory = factory;
            _interval = intervalSec;
        }
        
        public void Enable() => EventBus.Subscribe(HandleGameEvent);
        
        public void Disable() => EventBus.Unsubscribe(HandleGameEvent);
        
        public void Tick(float deltaTime)
        {
            _timer += deltaTime;
            if (_timer < _interval) return;
            _timer = 0f;
            Spawn();
        }

        private void Spawn()
        {
            _factory.SpawnOne();
            EventBus.Raise(GameEventType.EnemySpawned, 1);
        }
        
        private void HandleGameEvent(GameEventType type, int value)
        {
            // Подписчик по ТЗ
        }

    }
}