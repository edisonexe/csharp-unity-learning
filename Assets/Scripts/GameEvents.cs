using System;
using UnityEngine;

namespace DefaultNamespace
{
    public class GameEvents : MonoBehaviour
    {
        public event Action OnShotFired;
        public event Action OnTargetHit;
        public event Action OnTargetDestroyed;
        
        public void ShotFired() => OnShotFired?.Invoke();
        public void TargetHit() => OnTargetHit?.Invoke();
        public void TargetDestroyed() => OnTargetDestroyed?.Invoke();
    }
}