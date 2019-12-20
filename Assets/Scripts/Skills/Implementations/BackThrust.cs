using System.Linq;
using StatusEffects;
using StatusEffects.Implementations;
using UnityEngine;

namespace Skills.Implementations
{
    public class BackThrust : Skill
    {
        public override SkillId SkillId => SkillId.BackThrust;
        public override float Cooldown => 8;
        public override float SkillPointCost => 0;

        public override void Execute(PlayerController playerController)
        {
            var statusEffects = playerController.GetComponent<TargetableObject>().StatusEffects;
            statusEffects.Add(new BackThrustStatusEffect());
        }
    }
}