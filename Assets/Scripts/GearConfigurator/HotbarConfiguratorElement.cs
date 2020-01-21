using System.Collections;
using Settings.InputConfiguration;
using Skills;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class HotbarConfiguratorElement : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
    {
        public int Slot { get; private set; }
        public SkillId SkillId { get; private set; } = SkillId.None;
        
        private HotbarConfigurator _hotbarConfigurator;

        private Vector3 _mouseDownPosition;
        private Vector3 _initialPosition;

        private const float lerpSpeed = 10;

        [SerializeField] private Image skillIcon;
        [SerializeField] private TextMeshProUGUI hotkey;
        private MouseOverObject _mouseOver;
        
        private void Awake()
        {
            _hotbarConfigurator = transform.parent.GetComponent<HotbarConfigurator>();
            _mouseOver = GetComponentInChildren<MouseOverObject>();
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
            MouseOverDrawer.ClearMouseOver();
        }

        public void OnEndDrag(PointerEventData eventData)
        {
            var result = _hotbarConfigurator.ProcessEndDrag(this);
            if (result.resultType == HotbarDragResult.ResultType.Failed)
            {
                StartCoroutine(LerpToPositionCoroutine(_initialPosition));
            }
            else
            {
                StartCoroutine(LerpToPositionCoroutine(result.targetPosition));
            }
        }

        public void LerpToPosition(Vector3 targetPosition)
        {
            StartCoroutine(LerpToPositionCoroutine(targetPosition));
        }
        
        private IEnumerator LerpToPositionCoroutine(Vector3 targetPosition)
        {
            var startPos = transform.position;
            
            float i = 0;
            while (i < 1)
            {
                yield return new WaitForEndOfFrame();

                i += Time.deltaTime * lerpSpeed;

                transform.position = Vector3.Lerp(startPos, targetPosition, i);
            }

            transform.position = targetPosition;
        }

        public void SetSkill(SkillId skillId)
        {
            SkillId = skillId;

            if (skillId != SkillId.None)
            {
                var skill = SkillDictionary.GetSkill(skillId);
                skillIcon.sprite = skill.Icon;
                skillIcon.gameObject.SetActive(true);
                _mouseOver.SetData(skill.GenerateMouserOverData());
            }
            else
            {
                skillIcon.gameObject.SetActive(false);
                _mouseOver.SetData(null);
            }
        }

        public void SetSlot(int slot)
        {
            Slot = slot;
            hotkey.text = KeyBindings.Hotbar[Slot].primary.ToString().Replace("Alpha", "");
        }
    }
}