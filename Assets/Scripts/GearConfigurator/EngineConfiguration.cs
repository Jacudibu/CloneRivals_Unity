using Skills;
using UnityEngine;

namespace GearConfigurator
{
    [CreateAssetMenu(fileName = "engine", menuName = "GearConfigurations/EngineConfiguration")]
    public class EngineConfiguration : ScriptableObject
    {
        public SkillId[] skills;
        public float engineMinSpeedModifier;
        public float engineMaxSpeedModifier;
        public float engineBoostSpeedModifier;
        public float engineBoostTimeModifier;
    }
}