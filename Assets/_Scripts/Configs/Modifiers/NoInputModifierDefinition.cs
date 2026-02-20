using _Scripts.Gameplay.GameModifiers;
using _Scripts.Interfaces;
using UnityEngine;

namespace _Scripts.Configs.Modifiers
{
    [CreateAssetMenu(fileName = "NoInputModifier", menuName = "Configs/Modifiers/NoInputModifier Definition")]
    public class NoInputModifierDefinition : ModifierDefinition
    {
        [SerializeField][Min(0.1f)] private float _interval = 15f;
        [SerializeField][Min(0.1f)] private float _duration = 3f;

        public float Interval => _interval;
        public float Duration => _duration;
        
        public override IGameModifier Create(ModifierContext ctx)
        {
            return new NoInputModifier(ctx.InputToggle, _interval, _duration, ctx.Logger);
        }
    }
}