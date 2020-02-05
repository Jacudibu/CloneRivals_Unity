using Skills;
using UnityEngine;

namespace GearConfigurator
{
    public class GearConfiguration
    {
        public static GearConfiguration Current { get; } = new GearConfiguration();
        
        public EngineConfiguration engineConfiguration;
        public BoostConfiguration boostConfiguration = new BoostConfiguration();
        public SkillId[] hotbar;

        public EngineFlameConfiguration engineFlameConfiguration = new EngineFlameConfiguration();
    }

    public struct BoostConfiguration
    {
        public int boostSpeedModifier;
        public int boostTimeModifier;
    }

    public struct EngineFlameConfiguration
    {
        public Color innerFlameColor;
        public Color outerFlameColor;
    }
}