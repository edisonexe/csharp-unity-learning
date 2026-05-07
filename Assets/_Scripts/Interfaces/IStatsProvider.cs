using System;
using StressTest.Core;

namespace StressTest.Interfaces
{
    public interface IStatsProvider
    {
        event Action<SimulationStats> OnStatsChanged;
        void UpdateStats();
    }
}