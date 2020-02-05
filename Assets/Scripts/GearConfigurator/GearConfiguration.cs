using System.Drawing;
using Skills;

namespace GearConfigurator
{
    public class GearConfiguration
    {
        public static GearConfiguration Current { get; } = new GearConfiguration();
        
        public EngineConfiguration engineConfiguration;
        public BoostConfiguration boostConfiguration = new BoostConfiguration();
        public SkillId[] hotbar;

        public EngineFlameConfiguration engineFlameConfiguration;
    }

    public struct BoostConfiguration
    {
        public int boostSpeedModifier;
        public int boostTimeModifier;
    }

    public struct EngineFlameConfiguration
    {
        public Color innerFlame;
        public Color outerColor;
    }
}