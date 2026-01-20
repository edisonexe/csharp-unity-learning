using Actors.Enemy.AI;
using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField] private EnemyStrategyType _strategyType = EnemyStrategyType.Patrol;
    [SerializeField][Min(0)] private float _enemyMoveSpeed = 2f;
    public EnemyStrategyType StrategyType => _strategyType;
    public float EnemyMoveSpeed => _enemyMoveSpeed;
}
