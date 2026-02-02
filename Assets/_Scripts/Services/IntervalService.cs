using System;

namespace Services
{
    public class IntervalService
    {
        private readonly Action _tick;
        private readonly float _interval;
        private bool _enabled;
        private float _timer;

        public IntervalService(Action tick, float interval)
        {
            _tick = tick ?? throw new ArgumentNullException(nameof(tick));
            _interval = interval;
            _timer = interval / 2;
        }
        
        public void Enable()
        {
            if (_enabled) return;
            _enabled = true;
        }

        public void Disable()
        {
            if (!_enabled) return;
            _enabled = false;
        }

        public void Update(float dt)
        {
            if (!_enabled) return;
            _timer += dt;
            
            if (_timer < _interval) return;
            _timer -= _interval;
            _tick();
        }
        
    }
}