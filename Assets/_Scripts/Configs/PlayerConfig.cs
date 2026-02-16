using UnityEngine;

namespace _Scripts.Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/PlayerConfig")]
    public sealed class PlayerConfig : ScriptableObject
    {
        [SerializeField][Min(1)] private int _maxHp = 100;
        [SerializeField][Min(0.1f)] private float _moveSpeed = 5f;

        public int MaxHp => _maxHp;
        public float MoveSpeed => _moveSpeed;
    }
}