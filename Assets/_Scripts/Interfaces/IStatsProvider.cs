using System;
using _Scripts.Domain;

namespace _Scripts.Interfaces
{
    public interface IStatsProvider
    {
        event Action<SimulationStats> OnStatsChanged;
        void UpdateStats();
    }
}