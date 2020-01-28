namespace Skills.Implementations
{
    public class UTurn : Skill
    {
        public override string Name => "U Turn";
        public override string Description => "Instantly make a 180° turn.";
        public override SkillId SkillId => SkillId.UTurn;
        public override float Cooldown => 8;
        public override float SkillPointCost => 0;

        public override void Execute(PlayerController playerController)
        {
            playerController.transform.forward = -playerController.transform.forward;
        }
    }
}