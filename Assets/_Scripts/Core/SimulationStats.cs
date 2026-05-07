using StressTest.Enums;

namespace StressTest.Core
{
    public struct SimulationStats
    {
        private readonly ExecutionMode _mode;
        private readonly int _activeProjs;
        private readonly int _activeTargets;
        private readonly int _totalProjs;
        private readonly int _totalTargets;
        private readonly int _projPoolSize;
        private readonly int _targetPoolSize;
        private readonly int _projAvailable;
        private readonly int _targetAvailable;
        private readonly int _projReused;
        private readonly int _targetReused;
        
        public ExecutionMode Mode => _mode;
        public int ActiveProjs => _activeProjs;
        public int ActiveTargets => _activeTargets;
        public int TotalProjs => _totalProjs;
        public int TotalTargets => _totalTargets;
        
        public int ProjPoolSize => _projPoolSize;
        public int TargetPoolSize => _targetPoolSize;
        public int ProjAvailable => _projAvailable;
        public int TargetAvailable => _targetAvailable;
        public int ProjReused => _projReused;
        public int TargetReused => _targetReused;

        public SimulationStats(ExecutionMode mode, int activeProjs, int activeTargets, int totalProjs, 
            int totalTargets, int projPoolSize, int targetPoolSize, int projAvailable, int targetAvailable, 
            int projReused, int targetReused)
        {
            _mode = mode;
            _activeProjs = activeProjs;
            _activeTargets = activeTargets;
            _totalProjs = totalProjs;
            _totalTargets = totalTargets;
            _projPoolSize = projPoolSize;
            _targetPoolSize = targetPoolSize;
            _projAvailable = projAvailable;
            _targetAvailable = targetAvailable;
            _projReused = projReused;
            _targetReused = targetReused;
        }
    }
}