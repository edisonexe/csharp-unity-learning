using _Scripts.Gameplay.Controllers;
using _Scripts.Interfaces;
using StressTest.Enums;

namespace _Scripts.Gameplay.Services
{
    public class SimulationController : ISimulationController
    {
        private readonly EntityFactory _factory;
        private readonly StatsCollector _collector;
        private readonly TargetSpawner _spawner;

        private ExecutionMode _currentMode = ExecutionMode.Naive;

        public SimulationController(EntityFactory factory, StatsCollector collector, TargetSpawner spawner)
        {
            _factory = factory;
            _collector = collector;
            _spawner = spawner;
            ApplyMode(_currentMode);
        }

        public void ToggleMode()
        {
            _spawner.StopSpawning();
            _factory.Cleanup();

            _currentMode = _currentMode == ExecutionMode.Naive ? ExecutionMode.Pool : ExecutionMode.Naive;
            
            ApplyMode(_currentMode);
            _spawner.StartSpawning();
        }

        private void ApplyMode(ExecutionMode mode)
        {
            _factory.SetMode(mode);
            _factory.ResetCounter();
            _collector.SetMode(mode);
        }
    }
}