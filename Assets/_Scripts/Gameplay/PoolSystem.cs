using System;
using System.Collections.Generic;
using StressTest.Core;
using StressTest.Enums;
using StressTest.Interfaces;
using UnityEngine;

namespace StressTest.Gameplay
{
    public class PoolSystem : MonoBehaviour, IStatsProvider
    {
        [SerializeField] private ExecutionMode _mode;
        [SerializeField] private Projectile _projectilePrefab;
        [SerializeField] private Target _targetPrefab;
        [SerializeField] private int _prewarmCount = 100;

        public event Action<SimulationStats> OnStatsChanged;
        
        public ObjectPool<Projectile> ProjectilePool { get; private set; }
        public ObjectPool<Target> TargetPool { get; private set; }
        
        private readonly List<Projectile> _activeProjectiles = new();
        private readonly List<Target> _activeTargets = new();

        private int _totalProjsNaive;
        private int _totalTargetsNaive;
        
        public List<Target> ActiveTargets => _activeTargets;

        public void Init()
        {
            if (!_projectilePrefab) Debug.LogError("PoolSystem: Projectile Prefab is null!");
            if (!_targetPrefab) Debug.LogError("PoolSystem: Target Prefab is null!");

            ProjectilePool = new ObjectPool<Projectile>(_projectilePrefab, transform, _prewarmCount);
            TargetPool = new ObjectPool<Target>(_targetPrefab, transform, _prewarmCount);
            
            UpdateStats();
        }

        public void SpawnProjectile(Vector3 pos, Vector3 dir)
        {
            Projectile p;
            bool isPool = _mode == ExecutionMode.Pool;

            if (isPool)
            {
                p = ProjectilePool.Get();
            }
            else
            {
                p = Instantiate(_projectilePrefab);
                _totalProjsNaive++;
            }

            p.Init(pos, Quaternion.LookRotation(dir), dir, isPool, ReturnProjectile);
            _activeProjectiles.Add(p);
            
            UpdateStats();
        }

        public void SpawnTarget(Vector3 pos, Vector3 dir)
        {
            Target t;
            bool isPool = _mode == ExecutionMode.Pool;

            if (isPool)
            {
                t = TargetPool.Get();
            }
            else
            {
                t = Instantiate(_targetPrefab);
                _totalTargetsNaive++;
            }

            t.Init(pos, dir, isPool, ReturnTarget);
            _activeTargets.Add(t);
            
            UpdateStats();
        }

        private void ReturnTarget(Target t)
        {
            _activeTargets.Remove(t);
    
            if (_mode == ExecutionMode.Pool)
            {
                TargetPool.Return(t);
            }

            UpdateStats();
        }

        private void ReturnProjectile(Projectile p)
        {
            _activeProjectiles.Remove(p);
    
            if (_mode == ExecutionMode.Pool)
            {
                ProjectilePool.Return(p);
            }
    
            UpdateStats();
        }
        
        // private void ReturnProjectile(Projectile p)
        // {
        //     _activeProjectiles.Remove(p);
        //     
        //     if (_mode == ExecutionMode.Pool)
        //         ProjectilePool.Return(p);
        //     else
        //         Destroy(p.gameObject);
        //
        //     UpdateStats();
        // }
        //
        // private void ReturnTarget(Target t)
        // {
        //     _activeTargets.Remove(t);
        //     
        //     if (_mode == ExecutionMode.Pool)
        //         TargetPool.Return(t);
        //     else
        //         Destroy(t.gameObject);
        //
        //     UpdateStats();
        // }

        public void UpdateStats()
        {
            var stats = new SimulationStats(
                _mode,
                _activeProjectiles.Count,
                _activeTargets.Count,
                _mode == ExecutionMode.Pool ? ProjectilePool.TotalCreated : _totalProjsNaive,
                _mode == ExecutionMode.Pool ? TargetPool.TotalCreated : _totalTargetsNaive,
                ProjectilePool.TotalCreated,
                TargetPool.TotalCreated,
                ProjectilePool.AvailableCount,
                TargetPool.AvailableCount,
                ProjectilePool.ReusedCount,
                TargetPool.ReusedCount
            );

            OnStatsChanged?.Invoke(stats);
        }
    }
}