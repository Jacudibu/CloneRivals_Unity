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

            hotbarElementWidth = (int) ((RectTransform) elements[0].transform).rect.width;
            firstElementPosition = elements[0].transform.position;
        }


        public HotbarDragResult ProcessEndDrag(HotbarConfiguratorElement hotbarConfiguratorElement)
        {
            var rectTransform = (RectTransform) transform;

            if (!RectTransformUtility.RectangleContainsScreenPoint(rectTransform, Input.mousePosition))
            {
                Debug.Log("nope!");
                return HotbarDragResult.Failed;
            }

            var slot = (int) (Input.mousePosition.x - firstElementPosition.x) / hotbarElementWidth;
            Debug.Log("yup!" + slot);
            var result = new HotbarDragResult
            {
                resultType = HotbarDragResult.ResultType.Success,
                targetPosition = new Vector3(firstElementPosition.x + hotbarElementWidth * slot, firstElementPosition.y, 0)
            };

            // Todo: tell other thing to move
            
            return result;
        }
    }
}