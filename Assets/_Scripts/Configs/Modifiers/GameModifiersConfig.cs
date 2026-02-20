using System.Collections.Generic;
using UnityEngine;

namespace _Scripts.Configs.Modifiers
{
    [CreateAssetMenu(fileName = "GameModifiersConfig", menuName = "Configs/Modifiers/GameModifiers Config")]
    public class GameModifiersConfig : ScriptableObject
    {
        [SerializeField] private List<ModifierDefinition> _modifiers = new();
        public IReadOnlyList<ModifierDefinition> Modifiers => _modifiers;
    }
}