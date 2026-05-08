using System.Collections.Generic;
using _Scripts.Gameplay.Entities;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Gameplay.Systems
{
    [AddComponentMenu("StressTest/Systems/Projectile Manager")]
    public class ProjectileManager : MonoBehaviour, IUpdatableSystem
    {
        private List<Projectile> _projectiles;

        public void Init(List<Projectile> activeProjectiles)
        {
            _projectiles = activeProjectiles ?? throw new System.ArgumentNullException(nameof(activeProjectiles));
        }

        public void OnTick(float deltaTime)
        {
            for (var i = _projectiles.Count - 1; i >= 0; i--)
            {
                Projectile p = _projectiles[i];
                if (!p) continue;
                
                p.transform.Translate(p.Direction * (p.Speed * deltaTime), Space.World);
                
                p.CurrentLifetime += deltaTime;
                if (p.CurrentLifetime >= p.MaxLifetime)
                {
                    p.Despawn();
                }
            }
        }
    }
}