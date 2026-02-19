using System;

namespace _Scripts.Interfaces
{
    public interface IScore
    {
        int Current { get; }
        event Action<int> Changed;
        void Add(int amount);
        void Reset();
    }
}