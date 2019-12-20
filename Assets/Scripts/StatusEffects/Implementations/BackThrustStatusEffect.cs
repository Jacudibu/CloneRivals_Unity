using UnityEngine;

namespace StatusEffects.Implementations
{
    public class BackThrustStatusEffect : StatusEffect
    {
        public override float Duration => 1f;

        public override StatusEffectId StatusEffectId => StatusEffectId.ReverseEngine;

        public override bool OverrideCurrentSpeed => true;
        public override float ApplyCurrentSpeedOverride(float original)
        {
            return -100f;
        }
    }
}