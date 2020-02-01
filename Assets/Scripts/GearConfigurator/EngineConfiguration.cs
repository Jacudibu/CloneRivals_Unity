using System.Collections.Generic;
using System.Linq;
using Skills;
using UnityEngine;

namespace GearConfigurator
{
    [CreateAssetMenu(fileName = "engine", menuName = "GearConfigurations/EngineConfiguration")]
    public class EngineConfiguration : ScriptableObject
    {
        public string engineName;
        public Sprite sprite;
        public SkillId[] skills;
        public float minSpeedModifier;
        public float maxSpeedModifier;
        public float boostSpeedModifier;
        public float boostTimeModifier;
        [Multiline] public string description;

        public MouseOverData GetData()
        {
            var skillIcons = skills
                .Select(x => SkillDictionary.GetSkill(x).Icon)
                .ToArray();
            
            return new MouseOverData(engineName, skillIcons, GenerateDescription());
        }
        
        private string GenerateDescription()
        {
            var lines = new List<string>();

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