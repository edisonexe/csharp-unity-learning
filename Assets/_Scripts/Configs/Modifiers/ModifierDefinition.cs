using _Scripts.Gameplay.GameModifiers;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Configs.Modifiers
{
    public abstract class ModifierDefinition : ScriptableObject
    {
        [SerializeField] private string _displayName = "Default Name";
        public string DisplayName => _displayName;
        
        public abstract IGameModifier Create(ModifierContext ctx);
    }
}