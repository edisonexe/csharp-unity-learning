using _Scripts.Domain;
using _Scripts.Gameplay.Controllers;
using _Scripts.Gameplay.Services;
using _Scripts.Gameplay.Systems;
using _Scripts.UI;
using UnityEngine;

namespace _Scripts.Bootstrap
{
    [AddComponentMenu("StressTest/Bootstrap/Composition Root")]
    public class GameCompositionRoot : MonoBehaviour
    {
        [SerializeField] private PoolSystem _poolSystem;
        [SerializeField] private TurretController _turret;
        [SerializeField] private StressTestUI _ui;
        [SerializeField] private TargetSpawner _targetSpawner;
        
        [SerializeField] private ProjectileManager _projectileManager;
        [SerializeField] private TargetManager _targetManager;

        private void Awake()
        {
            ValidateDependencies();
            Bootstrap();
        }

        private void ValidateDependencies()
        {
            if (!_poolSystem) Debug.LogError("[CompositionRoot] PoolSystem is missing!");
            if (!_projectileManager) Debug.LogError("CompositionRoot: ProjectileManager missing!");
            if (!_targetManager) Debug.LogError("CompositionRoot: TargetManager missing!");
            if (!_turret) Debug.LogError("[CompositionRoot] TurretController is missing!");
            if (!_ui) Debug.LogError("[CompositionRoot] StressTestUI is missing!");
            if (!_targetSpawner) Debug.LogError("[CompositionRoot] TargetSpawner is missing!");
        }

        private void Bootstrap()
        {
            var registry = new EntityRegistry();
            _poolSystem.Init();
            
            var factory = new EntityFactory(_poolSystem, registry);
            var collector = new StatsCollector(_poolSystem, registry, factory);
            var controller = new SimulationController(factory, collector, _targetSpawner);
            
            _projectileManager.Init(registry.Projectiles);
            _targetManager.Init(registry.Targets);
            
            _turret.Init(factory, registry.Targets);
            _targetSpawner.Init(factory);
            _ui.Init(collector, controller);
            
            _targetSpawner.StartSpawning(); 
        }
        
        private void Update()
        {
            var dt = Time.deltaTime;
            
            _projectileManager.OnTick(dt);
            _targetManager.OnTick(dt);
        }
    }
}