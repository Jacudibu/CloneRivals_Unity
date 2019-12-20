using UnityEngine;

namespace StatusEffects
{
    public abstract class StatusEffect
    {
        public abstract StatusEffectId StatusEffectId { get; }
        
        public virtual float Duration => 0;
        public float RemainingDuration { get; protected set; }
        
        public virtual bool BlockAccelerating => false;
        public virtual bool BlockTurning => false;
        public virtual bool BlockRolling => false;
        public virtual bool BlockMovement => false;
        public virtual bool BlockAcceleration => false;

        public virtual bool OverrideCurrentSpeed => false;
        
        public virtual float ApplyCurrentSpeedOverride(float original)
        {
            return original;
        }
        
        public virtual float ModifyMinSpeed(float original)
        {
            return original;
        }
        
        public virtual float ModifyMaxSpeed(float original)
        {
            return original;
        }    
        
        public virtual float ModifyBoostSpeed(float original)
        {
            return original;
        }
    }
}