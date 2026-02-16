using System;

namespace _Scripts.Interfaces
{
    public interface IHealth
    {
        public int Current { get; }
        public int Max { get; }

        public event Action<int> Damaged;
        public event Action Died;

        public void Damage(int amount);
    }
}