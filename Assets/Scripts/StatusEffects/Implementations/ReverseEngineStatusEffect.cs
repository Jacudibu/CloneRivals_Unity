namespace StatusEffects.Implementations
{
    public class ReverseEngineStatusEffect : StatusEffect
    {
        public override StatusEffectId StatusEffectId => StatusEffectId.ReverseEngine;

        public override float ModifyMinSpeed(float original)
        {
            return -original;
        }
    }
}