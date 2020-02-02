using Settings.InputConfiguration;
using Skills;
using UnityEngine;

namespace UI
{
    public class SkillHotBar : MonoBehaviour
    {
        private SkillHotbarElement[] _hotbarElements;
        
        private void Start()
        {
            var playerController = FindObjectOfType<PlayerController>();
            playerController.OnSkillUsed += OnSkillUsed;

            _hotbarElements = GetComponentsInChildren<SkillHotbarElement>();

            for (var i = 0; i < _hotbarElements.Length; i++)
            {
                _hotbarElements[i].SetAssignedKey(KeyBindings.Hotbar[i]);
            }
            
            LoadConfiguration();
        }

        private void LoadConfiguration()
        {
            var config = GearConfigurator.GearConfiguration.Current;
            
            for (var i = 0; i < _hotbarElements.Length; i++)
            {
                _hotbarElements[i].LoadConfiguration(config.hotbar[i]);
            }
        }

        private void OnSkillUsed(SkillId skillId, int hotbarIndex, float cooldown)
        {
            var hotbarElement = _hotbarElements[hotbarIndex];
            hotbarElement.OnSkillUsed(cooldown);
        }
    }
}