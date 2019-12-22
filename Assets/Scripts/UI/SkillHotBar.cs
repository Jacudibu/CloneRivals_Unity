using InputConfiguration;
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

            _hotbarElements[0].SetAssignedKey(KeyBindings.Hotbar1);
            _hotbarElements[1].SetAssignedKey(KeyBindings.Hotbar2);
            _hotbarElements[2].SetAssignedKey(KeyBindings.Hotbar3);
        }

        private void OnSkillUsed(SkillId skillId, int hotbarIndex, float cooldown)
        {
            var hotbarElement = _hotbarElements[hotbarIndex];
            hotbarElement.OnSkillUsed(cooldown);
        }
    }
}