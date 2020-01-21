using System.Collections.Generic;
using GearConfigurator;
using UnityEngine;

namespace Skills
{
    public abstract class Skill
    {
        public abstract string Name { get; }
        public abstract SkillId SkillId { get; }
        public abstract float Cooldown { get; }
        public abstract float SkillPointCost { get; }

        public abstract void Execute(PlayerController playerController);

        public Sprite Icon { get; private set; }
        
        public void SetIcon(Sprite icon)
        {
            Icon = icon;
        }

        public MouseOverData GenerateMouserOverData()
        {
            var lines = new List<string>();

            if (SkillPointCost > 0)
            {
                lines.Add(SkillPointCost + " SP");
            }
            
            if (Cooldown > 0)
            {
                lines.Add(Cooldown + "s cooldown");
            }

            if (lines.Count == 0)
            {
                lines.Add("");
            }
            
            return new MouseOverData(Name, new []{Icon}, string.Join("\n", lines));
        }
    }
}
