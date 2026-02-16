using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Services.Logging
{
    public sealed class UnityLoggerService : ILoggerService
    {
        public void Log(string message) => Debug.Log(message);
        public void Warn(string message) => Debug.LogWarning(message);
        public void Error(string message) => Debug.LogError(message);
    }
}