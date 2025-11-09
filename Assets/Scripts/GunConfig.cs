using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GunConfig", menuName = "Configs/GunConfig")]
    public class GunConfig : ScriptableObject
    {
        [SerializeField][Min(0f)] private float _projectileSpeed = 20f;
        [SerializeField][Min(0f)] private float _fireCooldown = 0.5f;
        [SerializeField][Min(0f)] private int _damage = 10;
        
        public float ProjectileSpeed => _projectileSpeed;
        public float FireCooldown => _fireCooldown;
        public int Damage => _damage;
    }
}