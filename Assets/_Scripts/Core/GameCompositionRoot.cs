using UnityEngine;
using StressTest.Gameplay;
using StressTest.UI;

namespace StressTest.Core
{
    public class GameCompositionRoot : MonoBehaviour
    {
        [SerializeField] private PoolSystem _poolSystem;
        [SerializeField] private TurretController _turret;
        [SerializeField] private StressTestUI _ui;
        [SerializeField] private TargetSpawner _targetSpawner;

        private void Awake()
        {
            ValidateDependencies();
            Bootstrap();
        }

        private void ValidateDependencies()
        {
            if (!_poolSystem) Debug.LogError("[CompositionRoot] PoolSystem is missing!");
            if (!_turret) Debug.LogError("[CompositionRoot] TurretController is missing!");
            if (!_ui) Debug.LogError("[CompositionRoot] StressTestUI is missing!");
            if (!_targetSpawner) Debug.LogError("[CompositionRoot] TargetSpawner is missing!");
        }

        private void Bootstrap()
        {
            _poolSystem.Init();
            
            _turret.Init(_poolSystem, _poolSystem.ActiveTargets);
            _targetSpawner.Init(_poolSystem);

            _ui.Init(_poolSystem);
        }
    }
}