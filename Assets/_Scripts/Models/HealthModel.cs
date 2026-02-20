using System;
using _Scripts.Interfaces;

namespace _Scripts.Models
{
    public sealed class HealthModel : IHealth
    {
        public int Current { get; private set; }
        public int Max { get; }
        public event Action<int, int> Changed;
        public event Action Died;
        private ILoggerService _logger; 
    
        public HealthModel(int max)
        {
            Max = max;
            Current = max;
        }

        public void Damage(int amount)
        {
            Current = Math.Max(0, Current - amount);
            Changed?.Invoke(Current, Max);
        
            if (Current <= 0)
            {
                Current = 0;
                Died?.Invoke();
            }
        }

        public void Reset()
        {
            Current = Max;
            Changed?.Invoke(Current, Max);
        }

        public void Heal(int amount)
        {
            if (Current == Max) return;
            Current = Math.Min(Max, Current + amount);
            Changed?.Invoke(Current, Max);
        }
    }
}