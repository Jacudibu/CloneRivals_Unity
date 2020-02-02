using Skills;

namespace GearConfigurator
{
    public class GearConfiguration
    {
        public static GearConfiguration Current { get; } = new GearConfiguration();
        
        public EngineConfiguration engineConfiguration;
        public BoostConfiguration boostConfiguration = new BoostConfiguration();
        public SkillId[] hotbar;
    }

    public struct BoostConfiguration
    {
        public int boostSpeedModifier;
        public int boostTimeModifier;
    }
}