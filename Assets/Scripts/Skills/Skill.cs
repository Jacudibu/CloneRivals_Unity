namespace Skills
{
    public abstract class Skill
    {
        public abstract string Name { get; }
        public abstract SkillId SkillId { get; }
        public abstract float Cooldown { get; }
        public abstract float SkillPointCost { get; }

        public abstract void Execute(PlayerController playerController);
    }
}
