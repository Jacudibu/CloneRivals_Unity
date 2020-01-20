using System;
using UnityEngine;

namespace GearConfigurator
{
    public class HotbarConfigurator : MonoBehaviour
    {
        private int hotbarElementWidth;
        private Vector3 firstElementPosition;
        
        public HotbarConfiguratorElement[] elements;

        private void Start()
        {
            elements = GetComponentsInChildren<HotbarConfiguratorElement>();

            for (var i = 0; i < elements.Length; i++)
            {
                elements[i].slot = i;
            }

            hotbarElementWidth = (int) ((RectTransform) elements[0].transform).rect.width;
            firstElementPosition = elements[0].transform.position;
        }


        public HotbarDragResult ProcessEndDrag(HotbarConfiguratorElement element)
        {
            var rectTransform = (RectTransform) transform;

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                return HotbarDragResult.Failed;
            }

            var slot = (int) (Input.mousePosition.x - firstElementPosition.x) / hotbarElementWidth;
            var result = new HotbarDragResult
            {
                resultType = HotbarDragResult.ResultType.Success,
                targetPosition = new Vector3(firstElementPosition.x + hotbarElementWidth * slot, firstElementPosition.y, 0)
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

            other.LerpToPosition(new Vector3(firstElementPosition.x + hotbarElementWidth * other.slot, firstElementPosition.y, 0));
            
            return result;
        }
    }
}