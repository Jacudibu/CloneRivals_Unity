namespace Buffs
{
    public abstract class Buff
    {
        public abstract float Duration { get; }
        public float RemainingDuration { get; protected set; }
        
        public bool BlockAccelerating { get; } = false;
        public bool BlockTurning { get; } = false;
        public bool BlockRolling { get; } = false;
        public bool BlockMovement { get; } = false;
        public bool BlockAcceleration { get; } = false;

        public abstract void Tick(PlayerController playerController);
        
        
    }
}