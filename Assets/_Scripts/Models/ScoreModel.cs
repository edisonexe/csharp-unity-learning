using System;
using _Scripts.Interfaces;

namespace _Scripts.Models
{
    public class ScoreModel : IScore
    {
        public int Current { get; private set; }
        public event Action<int> Changed;

        public ScoreModel(int current = 0)
        {
            Current = current;
        }
        
        public void Add(int amount)
        {
            Current = Math.Max(Current, amount + Current);
            Changed?.Invoke(Current);
        }

        public void Reset()
        {
            Current = 0;
            Changed?.Invoke(Current);
        }
    }
}