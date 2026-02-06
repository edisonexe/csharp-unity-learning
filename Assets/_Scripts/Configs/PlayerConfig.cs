using UnityEngine;

namespace Configs
{
    [CreateAssetMenu(fileName = "PlayerConfig", menuName = "Configs/Player Config")]
    public class PlayerConfig : ScriptableObject
    {
        [SerializeField] private int _maxHp;
        [SerializeField] private int _damage;
        [SerializeField] private float _moveSpeed;
        [SerializeField] private WeaponConfig[] _weaponCfgs;
        
        public int MaxHp => _maxHp;
        public int Damage => _damage;
        public float MoveSpeed => _moveSpeed;
        public WeaponConfig[] WeaponCfgs => _weaponCfgs;
    }
}