using System.Collections.Generic;
using _Scripts.Gameplay.Entities;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.Systems
{
    [AddComponentMenu("StressTest/Systems/Target Manager")]
    public class TargetManager : MonoBehaviour, IUpdatableSystem
    {
        private List<Target> _targets;

        public void Init(List<Target> activeTargets)
        {
            _targets = activeTargets ?? throw new System.ArgumentNullException(nameof(activeTargets));
        }

        public void OnTick(float deltaTime)
        {
            for (var i = _targets.Count - 1; i >= 0; i--)
            {
                Target t = _targets[i];
                if (!t) continue;

                t.transform.Translate(t.MoveDirection * (t.Speed * deltaTime), Space.World);
            }
        }
    }
}