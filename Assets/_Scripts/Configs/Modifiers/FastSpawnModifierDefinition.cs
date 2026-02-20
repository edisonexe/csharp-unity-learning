using _Scripts.Gameplay.GameModifiers;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Configs.Modifiers
{
    [CreateAssetMenu(fileName = "FastSpawnModifier", menuName = "Configs/Modifiers/FastSpawnModifier Definition")]
    public class FastSpawnModifierDefinition : ModifierDefinition
    {
        [SerializeField][Min(1f)] private float _multiplier = 2f;

        public float Multiplier => _multiplier;
        
        public override IGameModifier Create(ModifierContext ctx)
        {
            return new FastSpawnModifier(ctx.SpawnRate, _multiplier, ctx.Logger);
        }
    }
}