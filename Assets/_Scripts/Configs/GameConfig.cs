using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
    public class GameConfig : ScriptableObject
    {
        [SerializeField][Min(0)] private int _enemiesPerWave = 3;
        [SerializeField][Min(0f)] private float _waveInterval = 10;
        [SerializeField][Min(1)] private int _maxWaves = 3;

        public float WaveInterval => _waveInterval;
        public int EnemiesPerWave => _enemiesPerWave;
        public int MaxWaves => _maxWaves;
    }
}