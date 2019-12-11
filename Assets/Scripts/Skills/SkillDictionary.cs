using System.Collections.Generic;

namespace Skills
{
    public static class SkillDictionary
    {
        private static readonly Dictionary<SkillId, Skill> Skills = new Dictionary<SkillId, Skill>
        {
            { SkillId.UTurn, new UTurn() } 
        };

        public static Skill GetSkill(SkillId skillId)
        {
            return Skills[skillId];
        }
    }
}