using System.Linq;
using StatusEffects;
using StatusEffects.Implementations;
using UnityEngine;

namespace Skills.Implementations
{
    public class ReverseEngine : Skill
    {
        public override string Name => "Reverse Engine";
        public override string Description => "Inverts your engines minimum speed, allowing you to fly backwards at will.";
        
        public override SkillId SkillId => SkillId.ReverseEngine;
        public override float Cooldown => 0;
        public override float SkillPointCost => 0;

        public override void Execute(PlayerController playerController)
        {
            var statusEffects = playerController.GetComponent<TargetableObject>().StatusEffects;

            if (statusEffects.Any(x => x.StatusEffectId == StatusEffectId.ReverseEngine))
            {
                statusEffects.RemoveAll(x => x.StatusEffectId == StatusEffectId.ReverseEngine);
            }
            else
            {
                statusEffects.Add(new ReverseEngineStatusEffect());
            }
        }
    }
}