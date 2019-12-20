using UI;
using UnityEngine;

namespace Assets.Scripts.UI
{
    public class SkillHotBar : MonoBehaviour
    {
        private SkillHotbarElement[] _hotbarElements;
        
        private void Start()
        {
            var playerController = FindObjectOfType<PlayerController>();
            playerController.OnSkillUsed += OnSkillUsed;

            _hotbarElements = GetComponentsInChildren<SkillHotbarElement>();
            
            _hotbarElements[0].SetAssignedKey(KeyCode.Alpha1);
            _hotbarElements[1].SetAssignedKey(KeyCode.Alpha2);
            _hotbarElements[2].SetAssignedKey(KeyCode.Alpha3);
        }

        private void OnSkillUsed(int index, float cooldown)
        {
            index -= 1;
            if (index < 0)
            {
                index = 10;
            }
            
            var hotbarElement = _hotbarElements[index];
            hotbarElement.OnSkillUsed(cooldown);
        }
    }
}