using _Scripts.Gameplay.GameModifiers;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Configs.Modifiers
{
    [CreateAssetMenu(fileName = "RegenModifier", menuName = "Configs/Modifiers/RegenModifier Definition")]
    public class RegenModifierDefinition : ModifierDefinition
    {
        [SerializeField][Min(0.1f)] private float _regenInterval = 2f;
        [SerializeField][Min(1f)] private int _regenAmount = 1;

        public float RegenInterval => _regenInterval;
        public float RegenAmount => _regenAmount;
        
        public override IGameModifier Create(ModifierContext ctx)
        {
            return new RegenModifier(ctx.Health, _regenInterval, _regenAmount, ctx.Logger);
        }
    }
}