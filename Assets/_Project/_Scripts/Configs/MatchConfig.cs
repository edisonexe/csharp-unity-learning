using UnityEngine;

namespace _Project._Scripts.Configs
{
    [CreateAssetMenu(fileName = "MatchConfig", menuName = "Configs/Match Config")]
    public sealed class MatchConfig : ScriptableObject
    {
        [SerializeField, Min(1)] private int _matchDurationSeconds = 300;
        
        public int MatchDurationSeconds => _matchDurationSeconds;
    }
}