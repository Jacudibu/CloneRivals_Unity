using System;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GearConfigurator
{
    public class HotbarConfiguratorElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        private HotbarConfigurator _hotbarConfigurator;
        
        private Vector3 _mouseDownPosition;
        private Vector3 _initialPosition;

        private const float lerpSpeed = 10;

        private void Start()
        {
            _hotbarConfigurator = transform.parent.GetComponent<HotbarConfigurator>();
        }

        public void OnBeginDrag(PointerEventData eventData)
        {
            _initialPosition = transform.position;
            _mouseDownPosition = Input.mousePosition;  
            
            transform.SetAsLastSibling();
        }

        public void OnDrag(PointerEventData eventData)
        {
            var delta = Input.mousePosition - _mouseDownPosition;
            transform.position = _initialPosition + delta;
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var result = _hotbarConfigurator.ProcessEndDrag(this);
            if (result.resultType == HotbarDragResult.ResultType.Failed)
            {
                StartCoroutine(LerpToPosition(_initialPosition));
            }
            else
            {
                StartCoroutine(LerpToPosition(result.targetPosition));
            }
        }

        private IEnumerator LerpToPosition(Vector3 targetPosition)
        {
            var startPos = transform.position;
            
            float i = 0;
            while (i < 1)
            {
                yield return null;

                i += Time.deltaTime * lerpSpeed;

                transform.position = Vector3.Lerp(startPos, targetPosition, i);
            }

            transform.position = targetPosition;
        }
    }
}