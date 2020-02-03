using System.Linq;
using Skills;
using UnityEngine;

namespace GearConfigurator
{
    public class HotbarConfigurator : MonoBehaviour
    {
        private int _hotbarElementWidth;
        private Vector3 _firstElementPosition;
        
        public HotbarConfiguratorElement[] elements;

        private void Start()
        {
            elements = GetComponentsInChildren<HotbarConfiguratorElement>();

            for (var i = 0; i < elements.Length; i++)
            {
                elements[i].SetSlot(i);
            }

            _hotbarElementWidth = (int) ((RectTransform) elements[0].transform).rect.width;
            _firstElementPosition = elements[0].transform.position;
        }


        public HotbarDragResult ProcessEndDrag(HotbarConfiguratorElement element)
        {
            if (!IsDragEndPositionValid((RectTransform) transform))
            {
                return HotbarDragResult.Failed;
            }

            var slot = (int) (Input.mousePosition.x - _firstElementPosition.x) / _hotbarElementWidth;
            var result = new HotbarDragResult
            {
                resultType = HotbarDragResult.ResultType.Success,
                targetPosition = GetHotbarPosition(slot)
            };

            var other = elements[slot];
            if (other == element)
            {
                return result;
            }

            elements[element.Slot] = other;
            elements[slot] = element;

            other.SetSlot(element.Slot);
            element.SetSlot(slot);

            other.LerpToPosition(GetHotbarPosition(other.Slot));
            
            return result;
        }

        private static bool IsDragEndPositionValid(RectTransform rectTransform)
        {
            return RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition);
        }

        private Vector3 GetHotbarPosition(int slot)
        {
            return new Vector3(_firstElementPosition.x + _hotbarElementWidth * slot, _firstElementPosition.y, 0);
        }

        public void SwapSkills(SkillId[] oldSkills, SkillId[] newSkills)
        {
            var skillsToRemove = oldSkills.Except(newSkills);
            var skillsToAdd = newSkills.Except(oldSkills);
            
            foreach (var skillId in skillsToRemove)
            {
                RemoveSkill(skillId);
            }
            
            foreach (var skillId in skillsToAdd)
            {
                AddSkill(skillId);
            }
        }
        
        public void RemoveSkill(SkillId skillId)
        {
            var element = elements.SingleOrDefault(x => x.SkillId == skillId);
            if (element == null)
            {
                return;
            }
            
            element.SetSkill(SkillId.None);
        }

        public void AddSkill(SkillId skillId)
        {
            var i = 0;
            while (elements[i].SkillId != SkillId.None)
            {
                i++;
            }

            elements[i].SetSkill(skillId);
        }
    }
}