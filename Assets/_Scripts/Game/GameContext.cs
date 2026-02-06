using System;

namespace Game
{

    public sealed class GameContext
    {
        public int Kills { get; private set; }
        public int AliveEnemies { get; private set; }
        
        private bool _allWavesSpawned;
        private bool _winTriggered;

        public event Action<int> KillsChanged;
        public event Action OnWin;
        
        
        public void EnemyKilled()
        {
            AliveEnemies--;
            Kills++;
            KillsChanged?.Invoke(Kills);
            CheckWinCondition();
        }
        
        public void MarkAllWavesSpawned() => _allWavesSpawned = true;

        private void CheckWinCondition()
        {
            if (_winTriggered)
                return;

            if (_allWavesSpawned && AliveEnemies == 0)
            {
                _winTriggered = true;
                OnWin?.Invoke();
            }
        }
        
        public void EnemySpawned() => AliveEnemies++;
        
        public void Reset()
        {
            Kills = 0;
            AliveEnemies = 0;
            _winTriggered = false;
            KillsChanged?.Invoke(Kills);
        }
    }

}