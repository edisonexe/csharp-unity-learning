using System.Collections.Generic;
using _Scripts.Interfaces;

namespace _Scripts.Gameplay.GameModifiers
{
    public sealed class ModifierRunner
    {
        private readonly IReadOnlyList<IGameModifier> _modifiers;

        private bool _active;

        public ModifierRunner(IReadOnlyList<IGameModifier> modifiers)
        {
            _modifiers = modifiers;
        }

        public void Activate()
        {
            if (_active) return;
            _active = true;

            for (var i = 0; i < _modifiers.Count; i++)
                _modifiers[i].OnEnterGameplay();
        }

        public void Deactivate()
        {
            if (!_active) return;
            _active = false;

            for (var i = 0; i < _modifiers.Count; i++)
                _modifiers[i].OnExitGameplay();
        }

        public void Tick(float dt)
        {
            if (!_active) return;

            for (var i = 0; i < _modifiers.Count; i++)
                _modifiers[i].Tick(dt);
        }
    }
}