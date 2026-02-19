using UnityEngine;

namespace _Scripts.Configs
{
    [CreateAssetMenu(fileName = "GameRulesConfig", menuName = "Configs/Game Rules Config")]
    public class GameRulesConfig : ScriptableObject
    {
        [SerializeField] [Min(10)] private int _targetScore = 80;

        public int TargetScore => _targetScore;
    }
}