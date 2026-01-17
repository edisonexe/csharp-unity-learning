using UnityEngine;

[CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
public class EnemyConfig : ScriptableObject
{
    [SerializeField][Min(0)] private float _enemyMoveSpeed = 1.5f;
    public float EnemyMoveSpeed => _enemyMoveSpeed;
}
