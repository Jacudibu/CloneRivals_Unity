using System.Collections.Generic;
using Skills;
using UnityEngine;

namespace GearConfigurator
{
    [CreateAssetMenu(fileName = "engine", menuName = "GearConfigurations/EngineConfiguration")]
    public class EngineConfiguration : ScriptableObject
    {
        public string engineName;
        public SkillId[] skills;
        public int minSpeedModifier;
        public int maxSpeedModifier;
        public int boostSpeedModifier;
        public int boostTimeModifier;
        [Multiline] public string description;

        public MouseOverData GetData()
        {
            return new MouseOverData(engineName, GenerateDescription());
        }
        
        private string GenerateDescription()
        {
            var lines = new List<string>();
            
            if (skills.Length > 0)
            {
                lines.Add("Skills:");
                foreach (var skillId in skills)
                {
                    var skill = SkillDictionary.GetSkill(skillId);
                    lines.Add(skill.Name);
                }
                lines.Add("");
            }

            if (minSpeedModifier != 0)
            {
                lines.Add("Minimum Speed " + minSpeedModifier.ToString("+#;-#;0"));
            }            
            
            if (maxSpeedModifier != 0)
            {
                lines.Add("Maximum Speed " + maxSpeedModifier.ToString("+#;-#;0"));
            }            
            
            if (boostSpeedModifier != 0)
            {
                lines.Add("Boost Speed " + boostSpeedModifier.ToString("+#;-#;0"));
            }            
            
            if (boostTimeModifier != 0)
            {
                lines.Add("Boost Time " + boostTimeModifier.ToString("+#;-#;0"));
            }            
            
            lines.Add("");
            lines.Add(description);
            
            return string.Join("\n", lines);
        }
    }
}