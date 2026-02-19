using UnityEngine;

namespace _Scripts.Configs
{
    [CreateAssetMenu(fileName = "SpawnerConfig", menuName = "Configs/Spawner Config")]
    public class SpawnerConfig : ScriptableObject
    {
        [SerializeField][Min(0.1f)] private float _interval = 1.5f;
        [SerializeField][Min(1)] private int _maxTotal = 7;
            
        public float Interval => _interval;
        public int MaxTotal => _maxTotal;
    }
}