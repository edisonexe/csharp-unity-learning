using UnityEngine;

namespace _Project._Scripts.Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Weapon Config")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField, Min(0)] private int _damage = 10;
        [SerializeField, Range(0.01f, 1f)] private float _fireRate = 0.4f;
        [SerializeField, Min(1f)] private float _range = 100f;

        public int Damage => _damage;
        public float FireRate => _fireRate;
        public float Range => _range;
    }
}
