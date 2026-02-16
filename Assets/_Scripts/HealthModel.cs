using System;
using _Scripts.Interfaces;
using NUnit.Framework.Internal;

public sealed class HealthModel : IHealth
{
    public int Current { get; private set; }
    public int Max { get; }
    public event Action<int> Damaged;
    public event Action Died;
    private ILoggerService _logger; 
    
    public HealthModel(int max, ILoggerService logger)
    {
        Max = max;
        Current = max;
    }

    public void Damage(int amount)
    {
        if (Current <= 0) return;

        Current -= amount;
        Damaged?.Invoke(amount);

        if (Current <= 0)
        {
            Current = 0;
            Died?.Invoke();
        }
    }
}