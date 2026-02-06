using UnityEngine;
using Weapon;

namespace Configs
{
    [CreateAssetMenu(fileName = "WeaponConfig", menuName = "Configs/Weapon Config")]
    public class WeaponConfig : ScriptableObject
    {
        [SerializeField] private string _name = "Default";
        [SerializeField] private Sprite _icon;
        [SerializeField] private int _damage = 1;
        [SerializeField] private float _fireRate = 0.15f;
        [SerializeField] private int _maxAmmo = 40;
        [SerializeField] private float _range = 30f;
        [SerializeField] private FireMode _fireMode = FireMode.Single;
        
        public string Name => _name;
        public Sprite Icon => _icon;
        public int Damage => _damage;
        public float FireRate => _fireRate;
        public int MaxAmmo => _maxAmmo;
        public float Range => _range;
        public FireMode FireMode => _fireMode;
    }
}
