using UnityEngine;

namespace StatusEffects.Implementations
{
    public class BackThrustStatusEffect : StatusEffect
    {
        public override float Duration => 1.5f;

        public override StatusEffectId StatusEffectId => StatusEffectId.ReverseEngine;

        private float _initialSpeed;
        private Engine _engine;
        
        public override bool OverrideCurrentSpeed => true;
        public override bool BlockRolling => true;
        public override bool BlockTurning => true;
        public override bool CancelOnCollision => true;

        public override void Initialize(TargetableObject targetableObject)
        {
            _engine = targetableObject.GetComponent<Engine>();
            _initialSpeed = _engine.currentSpeed;
        }

        public override float ApplyCurrentSpeedOverride(float original)
        {
            return -100f;
        }

        public override void OnEffectEnd()
        {
            _engine.currentSpeed = _initialSpeed;
        }
    }
}