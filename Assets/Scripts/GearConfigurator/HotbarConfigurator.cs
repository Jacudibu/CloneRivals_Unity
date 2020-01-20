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
                elements[i].slot = i;
            }

            _hotbarElementWidth = (int) ((RectTransform) elements[0].transform).rect.width;
            _firstElementPosition = elements[0].transform.position;
        }


        public HotbarDragResult ProcessEndDrag(HotbarConfiguratorElement element)
        {
            var rectTransform = (RectTransform) transform;

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
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

            elements[element.slot] = other;
            elements[slot] = element;

            other.slot = element.slot;
            element.slot = slot;

            other.LerpToPosition(GetHotbarPosition(other.slot));
            
            return result;
        }

        private Vector3 GetHotbarPosition(int slot)
        {
            return new Vector3(_firstElementPosition.x + _hotbarElementWidth * slot, _firstElementPosition.y, 0);
        }

        public void RemoveSkill(SkillId skillId)
        {
            var element = elements.Single(x => x.SkillId == skillId);
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