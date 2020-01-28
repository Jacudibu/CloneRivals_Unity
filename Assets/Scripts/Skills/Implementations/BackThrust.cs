using System.Linq;
using StatusEffects;
using StatusEffects.Implementations;
using UnityEngine;

namespace Skills.Implementations
{
    public class BackThrust : Skill
    {
        public override string Name => "Back Thrust";
        public override string Description => "Instantly propels you backwards.";
        public override SkillId SkillId => SkillId.BackThrust;
        public override float Cooldown => 8;
        public override float SkillPointCost => 0;

        public override void Execute(PlayerController playerController)
        {
            var targetable = playerController.GetComponent<TargetableObject>();
            var statusEffects = targetable.StatusEffects;
            
            var effect = new BackThrustStatusEffect();
            effect.Initialize(targetable);
            statusEffects.Add(effect);
        }
    }
}