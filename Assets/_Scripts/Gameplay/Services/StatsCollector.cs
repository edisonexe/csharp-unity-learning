using System;
using _Scripts.Domain;
using _Scripts.Gameplay.Systems;
using _Scripts.Interfaces;
using StressTest.Enums;

namespace _Scripts.Gameplay.Services
{
    public class StatsCollector : IStatsProvider
    {
        private readonly PoolSystem _pools;
        private readonly EntityRegistry _registry;
        private readonly EntityFactory _entityFactory;
        private ExecutionMode _currentMode;
        public event Action<SimulationStats> OnStatsChanged;

        public StatsCollector(PoolSystem pools, EntityRegistry registry, EntityFactory entityFactory)
        {
            _pools = pools;
            _registry = registry;
            _entityFactory = entityFactory;
            _registry.OnChanged += UpdateStats;
        }

        public void SetMode(ExecutionMode mode) { _currentMode = mode; UpdateStats(); }

        public void UpdateStats()
        {
            bool isPool = _currentMode == ExecutionMode.Pool;
            int totalProjs = isPool ? _pools.ProjectilePool.TotalCreated : _entityFactory.ProjNaiveCounter;
            int totalTargets = isPool ? _pools.TargetPool.TotalCreated : _entityFactory.TargetNaiveCounter;

            var stats = new SimulationStats(
                _currentMode, 
                _registry.Projectiles.Count, 
                _registry.Targets.Count, 
                totalProjs,
                totalTargets,
                _pools.ProjectilePool.TotalCreated, 
                _pools.TargetPool.TotalCreated,
                _pools.ProjectilePool.AvailableCount, 
                _pools.TargetPool.AvailableCount,
                _pools.ProjectilePool.ReusedCount,
                _pools.TargetPool.ReusedCount
            );
            OnStatsChanged?.Invoke(stats);
        }
    }
}