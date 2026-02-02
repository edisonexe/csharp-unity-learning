using System;

namespace Game
{

    public sealed class GameContext
    {
        public int Kills { get; private set; }

        public event Action<int> KillsChanged;

        public void AddKill()
        {
            Kills++;
            KillsChanged?.Invoke(Kills);
        }

        public void Reset()
        {
            Kills = 0;
            KillsChanged?.Invoke(Kills);
        }
    }

}