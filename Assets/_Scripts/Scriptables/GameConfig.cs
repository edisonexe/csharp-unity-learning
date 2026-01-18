using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Configs/Game Config")]
public class GameConfig : ScriptableObject
{
    [SerializeField][Min(0)] private float _playerMoveSpeed = 2f;
    [SerializeField][Min(0)] private float _enemySpawnInterval = 4f;
    [SerializeField][Min(0)] private int _targetScore = 500;
    [SerializeField] private float _itemSpawnInterval = 3f;

    public int TargetScore => _targetScore;
    public float EnemySpawnInterval => _enemySpawnInterval;
    public float PlayerMoveSpeed => _playerMoveSpeed;
    public float ItemSpawnInterval => _itemSpawnInterval;
}
