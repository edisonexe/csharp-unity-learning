using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "EnemyConfig", menuName = "Configs/Enemy Config")]
    public class EnemyConfig : ScriptableObject
    {
        [SerializeField][Min(1)] private int _maxHp;
        [SerializeField][Min(1f)] private float _moveSpeed = 2f;
        [SerializeField][Min(1f)] private int _damage = 1;
        [SerializeField][Min(0.2f)] private float _attackCooldown = 0.8f;
        [SerializeField] [Min(0.2f)] private float _attackDistance = 0.5f;
        [SerializeField] private Material _enemyMaterial;

        public int MaxHp => _maxHp;
        public float MoveSpeed => _moveSpeed;
        public int Damage => _damage;
        public float AttackCooldown => _attackCooldown;
        public float AttackDistance => _attackDistance;
        public Material EnemyMaterial => _enemyMaterial;
    }
}