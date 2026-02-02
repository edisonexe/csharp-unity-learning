using Interfaces;

namespace Spawning
{
    public class WaveSpawner
    {
        private readonly ISpawner _spawner;
        private readonly int _countPerWave;
        private readonly int _maxWaves;
        private int _spawnedWaves;

        public WaveSpawner(ISpawner spawner, int countPerWave, int maxWaves)
        {
            _spawner = spawner;
            _countPerWave = countPerWave;
            _maxWaves = maxWaves;
        }

        private bool CanSpawnWave() => _spawnedWaves < _maxWaves;
        
        public void SpawnWave()
        {
            if (!CanSpawnWave()) return;
            
            for (var i = 0; i < _countPerWave; i++)
            {
                _spawner.SpawnOne();
            }
            
            _spawnedWaves++;
        }

    }
}