using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField][Min(0)] private float _playerMoveSpeed = 2f;
    [SerializeField][Min(0)] private float _enemySpawnInterval = 2.5f;
    [SerializeField][Min(0)] private int _targetScore = 500;

    public int TargetScore => _targetScore;
    public float EnemySpawnInterval => _enemySpawnInterval;
    public float PlayerMoveSpeed => _playerMoveSpeed;
}
