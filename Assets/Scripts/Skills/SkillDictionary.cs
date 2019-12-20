using System.Collections.Generic;
using Skills.Implementations;

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

        public static Skill GetSkill(SkillId skillId)
        {
            return Skills[skillId];
        }
    }
}