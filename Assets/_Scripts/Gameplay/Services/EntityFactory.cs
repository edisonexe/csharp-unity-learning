using _Scripts.Domain;
using _Scripts.Gameplay.Entities;
using _Scripts.Gameplay.Systems;
using StressTest.Enums;
using UnityEngine;

namespace _Scripts.Gameplay.Services
{
    public class EntityFactory
    {
        private readonly PoolSystem _pools;
        private readonly EntityRegistry _registry;
        
        private ExecutionMode _mode;
        private int _projNaiveCounter;
        private int _targetNaiveCounter;

        public int ProjNaiveCounter => _projNaiveCounter;
        public int TargetNaiveCounter => _targetNaiveCounter;

        public EntityFactory(PoolSystem pools, EntityRegistry registry)
        {
            _pools = pools;
            _registry = registry;
        }

        public void SetMode(ExecutionMode mode) => _mode = mode;

        public void ResetCounter()
        {
            _projNaiveCounter = 0; 
            _targetNaiveCounter = 0; 
        }

        public Projectile CreateProjectile(Vector3 pos, Quaternion rot, Vector3 dir)
        {
            Projectile p = _mode == ExecutionMode.Pool 
                ? _pools.ProjectilePool.Get() 
                : Object.Instantiate(_pools.ProjectilePool.Prefab);

            if (_mode == ExecutionMode.Naive) _projNaiveCounter++;

            p.Init(pos, rot, dir, ReleaseProjectile);
            _registry.AddProjectile(p);
            return p;
        }

        public Target CreateTarget(Vector3 pos, Vector3 dir)
        {
            Target t = _mode == ExecutionMode.Pool 
                ? _pools.TargetPool.Get() 
                : Object.Instantiate(_pools.TargetPool.Prefab);

            if (_mode == ExecutionMode.Naive) _targetNaiveCounter++;

            t.Init(pos, dir, ReleaseTarget);
            _registry.AddTarget(t);
            return t;
        }

        private void ReleaseProjectile(Projectile p)
        {
            if (!_registry.Projectiles.Contains(p)) return;
            _registry.RemoveProjectile(p);

            if (_mode == ExecutionMode.Pool) _pools.ProjectilePool.Return(p);
            else Object.Destroy(p.gameObject);
        }

        private void ReleaseTarget(Target t)
        {
            if (!_registry.Targets.Contains(t)) return;
            _registry.RemoveTarget(t);

            if (_mode == ExecutionMode.Pool) _pools.TargetPool.Return(t);
            else Object.Destroy(t.gameObject);
        }

        public void Cleanup()
        {
            for (var i = _registry.Projectiles.Count - 1; i >= 0; i--) 
                ReleaseProjectile(_registry.Projectiles[i]);
            for (var i = _registry.Targets.Count - 1; i >= 0; i--) 
                ReleaseTarget(_registry.Targets[i]);
            _registry.Clear();
        }
    }
}