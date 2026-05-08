using System.Collections.Generic;
using _Scripts.Gameplay.Entities;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.Systems
{
    [AddComponentMenu("StressTest/Systems/Projectile Manager")]
    public class ProjectileManager : MonoBehaviour, IUpdatableSystem
    {
        private HashSet<Projectile> _projectiles;
        private readonly List<Projectile> _toRemoveBuffer = new(512);

        public void Init(HashSet<Projectile> activeProjectiles)
        {
            _projectiles = activeProjectiles ?? throw new System.ArgumentNullException(nameof(activeProjectiles));
        }

        public void OnTick(float deltaTime)
        {
            _toRemoveBuffer.Clear();

            foreach (var p in _projectiles)
            {
                if (!p) continue;
                
                p.transform.Translate(p.Direction * (p.Speed * deltaTime), Space.World);
                
                p.CurrentLifetime += deltaTime;
                if (p.CurrentLifetime >= p.MaxLifetime)
                {
                    _toRemoveBuffer.Add(p);
                }
            }
            
            for (var i = 0; i < _toRemoveBuffer.Count; i++)
            {
                _toRemoveBuffer[i].Despawn();
            }
        }
    }
}