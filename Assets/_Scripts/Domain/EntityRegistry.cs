using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Entities;

namespace _Scripts.Domain
{
    public class EntityRegistry
    {
        public event Action OnChanged;

        private readonly List<Projectile> _projectiles = new ();
        private readonly List<Target> _targets = new ();

        public List<Projectile> Projectiles => _projectiles;
        public List<Target> Targets => _targets;

        public void AddProjectile(Projectile p)
        {
            _projectiles.Add(p);
            OnChanged?.Invoke();
        }

        public void RemoveProjectile(Projectile p)
        {
            _projectiles.Remove(p);
            OnChanged?.Invoke();
        }

        public void AddTarget(Target t)
        {
            _targets.Add(t);
            OnChanged?.Invoke();
        }

        public void RemoveTarget(Target t)
        {
            _targets.Remove(t);
            OnChanged?.Invoke();
        }

        public void Clear()
        {
            _projectiles.Clear();
            _targets.Clear();
            OnChanged?.Invoke();
        }
    }
}