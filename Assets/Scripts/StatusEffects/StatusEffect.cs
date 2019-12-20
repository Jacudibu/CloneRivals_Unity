namespace StatusEffects
{
    public abstract class StatusEffect
    {
        public abstract StatusEffectId StatusEffectId { get; }
        
        public virtual float Duration => 0;
        public float RemainingDuration { get; protected set; }
        
        public bool BlockAccelerating { get; } = false;
        public bool BlockTurning { get; } = false;
        public bool BlockRolling { get; } = false;
        public bool BlockMovement { get; } = false;
        public bool BlockAcceleration { get; } = false;

        public virtual void Tick(PlayerController playerController)
        {
            
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