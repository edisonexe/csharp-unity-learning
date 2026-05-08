using _Scripts.Gameplay.Entities;
using _Scripts.Infrastructure;
using UnityEngine;

namespace _Scripts.Gameplay.Systems
{
    [AddComponentMenu("StressTest/Systems/Pool System")]
    public class PoolSystem : MonoBehaviour
    {
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Target _targetPrefab;
        [SerializeField] private int _prewarmCount = 500;

        public ObjectPool<Projectile> ProjectilePool { get; private set; }
        public ObjectPool<Target> TargetPool { get; private set; }

        public void Init()
        {
            ProjectilePool = new ObjectPool<Projectile>(_projectilePrefab, transform, _prewarmCount);
            TargetPool = new ObjectPool<Target>(_targetPrefab, transform, _prewarmCount);
        }
    }
}