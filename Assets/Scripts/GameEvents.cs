using System;
using UnityEngine;

namespace DefaultNamespace
{
    [CreateAssetMenu(fileName = "GameEvents", menuName = "Configs/GameEvents")]
    public class GameEvents : ScriptableObject
    {
        public event Action OnTargetSpawned;
        public event Action OnShotFired;
        public event Action OnTargetHit;

        
        public void RaiseTargetSpawned() => OnTargetSpawned?.Invoke();
        public void RaiseShotFired() => OnShotFired?.Invoke();
        public void RaiseTargetHit() => OnTargetHit?.Invoke();
    }
}