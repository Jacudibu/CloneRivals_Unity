namespace Skills
{
    public class UTurn : Skill
    {
        public override SkillId SkillId => SkillId.UTurn;
        public override float Cooldown => 15;
        public override float SkillPointCost => 30;

        public override void Execute(PlayerController playerController)
        {
            playerController.transform.forward = -playerController.transform.forward;
        }
    }
}