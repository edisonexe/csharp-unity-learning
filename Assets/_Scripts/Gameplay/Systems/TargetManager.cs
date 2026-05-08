using System.Collections.Generic;
using _Scripts.Gameplay.Entities;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.Systems
{
    [AddComponentMenu("StressTest/Systems/Target Manager")]
    public class TargetManager : MonoBehaviour, IUpdatableSystem
    {
        private HashSet<Target> _targets;

        public void Init(HashSet<Target> activeTargets)
        {
            _targets = activeTargets ?? throw new System.ArgumentNullException(nameof(activeTargets));
        }

        public void OnTick(float deltaTime)
        {
            foreach (var t in _targets)
            {
                if (!t) continue;
                t.transform.Translate(t.MoveDirection * (t.Speed * deltaTime), Space.World);
            }
        }
    }
}