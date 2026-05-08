using System;
using _Scripts.Domain;
using _Scripts.Gameplay.Entities;
using _Scripts.Gameplay.Systems;
using StressTest.Enums;
using UnityEngine;
using Object = UnityEngine.Object;

namespace _Scripts.Gameplay.Services
{
    public class EntityFactory
    {
        private readonly PoolSystem _pools;
        private readonly EntityRegistry _registry;
        private readonly Action<Projectile> _projectileReleaseCache;
        private readonly Action<Target> _targetReleaseCache;

        private ExecutionMode _mode;
        private int _projNaiveCounter;
        private int _targetNaiveCounter;

        public int ProjNaiveCounter => _projNaiveCounter;
        public int TargetNaiveCounter => _targetNaiveCounter;

        public EntityFactory(PoolSystem pools, EntityRegistry registry)
        {
            _pools = pools ?? throw new ArgumentNullException(nameof(pools));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));

            _projectileReleaseCache = ReleaseProjectile;
            _targetReleaseCache = ReleaseTarget;
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

            p.Init(pos, rot, dir, _projectileReleaseCache);
            _registry.AddProjectile(p);
            return p;
        }

        public Target CreateTarget(Vector3 pos, Vector3 dir)
        {
            Target t = _mode == ExecutionMode.Pool
                ? _pools.TargetPool.Get()
                : Object.Instantiate(_pools.TargetPool.Prefab);

            if (_mode == ExecutionMode.Naive) _targetNaiveCounter++;

            t.Init(pos, dir, _targetReleaseCache);
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
            while (_registry.Projectiles.Count > 0)
            {
                var enumerator = _registry.Projectiles.GetEnumerator();
                enumerator.MoveNext();
                ReleaseProjectile(enumerator.Current);
                enumerator.Dispose();
            }

            while (_registry.Targets.Count > 0)
            {
                var enumerator = _registry.Targets.GetEnumerator();
                enumerator.MoveNext();
                ReleaseTarget(enumerator.Current);
                enumerator.Dispose();
            }

            _registry.Clear();
        }
    }
}