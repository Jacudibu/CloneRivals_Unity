using System.Collections.Generic;
using Skills.Implementations;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

namespace Skills
{
    public static class SkillDictionary
    {
        private static readonly Dictionary<SkillId, Skill> Skills = new Dictionary<SkillId, Skill>
        {
            { SkillId.UTurn, new UTurn() } ,
            { SkillId.ReverseEngine, new ReverseEngine() },
            { SkillId.BackThrust, new BackThrust() },
        };

        static SkillDictionary()
        {
            foreach (var skill in Skills.Values)
            {
                var icon = Resources.Load<Sprite>($"SkillIcons/{skill.GetType().Name}");
                
                if (icon == null)
                {
                    Debug.LogError("Unable to load Skill Icon for " + skill.Name);
                }
                
                skill.SetIcon(icon);
            }
        }
        
        public static Skill GetSkill(SkillId skillId)
        {
            return Skills[skillId];
        }
    }
}