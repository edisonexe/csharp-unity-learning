using System;
using System.Collections.Generic;
using _Scripts.Gameplay.Entities;

namespace _Scripts.Domain
{
    public class EntityRegistry
    {
        public event Action OnChanged;

        private readonly HashSet<Projectile> _projectiles = new();
        private readonly HashSet<Target> _targets = new();

        public HashSet<Projectile> Projectiles => _projectiles;
        public HashSet<Target> Targets => _targets;

        public void AddProjectile(Projectile p)
        {
            if (_projectiles.Add(p)) OnChanged?.Invoke();
        }

        public void RemoveProjectile(Projectile p)
        {
            if (_projectiles.Remove(p)) OnChanged?.Invoke();
        }

        public void AddTarget(Target t)
        {
            if (_targets.Add(t)) OnChanged?.Invoke();
        }

        public void RemoveTarget(Target t)
        {
            if (_targets.Remove(t)) OnChanged?.Invoke();
        }

        public void Clear()
        {
            _projectiles.Clear();
            _targets.Clear();
            OnChanged?.Invoke();
        }
    }
}