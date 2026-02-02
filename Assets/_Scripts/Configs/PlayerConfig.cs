using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private int _maxHp;
        [SerializeField] private int _damage;
        [SerializeField] private float _moveSpeed;

        public int MaxHp => _maxHp;
        public int Damage => _damage;
        public float MoveSpeed => _moveSpeed;
    }
}