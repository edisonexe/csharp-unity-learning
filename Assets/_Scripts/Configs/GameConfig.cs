using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField][Min(0)] private float _playerMoveSpeed = 6f;
    [SerializeField][Min(0.5f)] private float _enemySpawnInterval = 5f;
    [SerializeField][Min(0)] private int _targetScore = 500;
    [SerializeField][Min(0.5f)] private float _itemSpawnInterval = 3f;
    [SerializeField][Min(1)] private int _playerMaxHp = 3;

    public int TargetScore => _targetScore;
    public float EnemySpawnInterval => _enemySpawnInterval;
    public float PlayerMoveSpeed => _playerMoveSpeed;
    public float ItemSpawnInterval => _itemSpawnInterval;
    public int PlayerMaxHp => _playerMaxHp;
}
