using System;
using System.Collections;
using InputConfiguration;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillHotbarElement : MonoBehaviour
    {
        [SerializeField] private Image cooldownFillerImage;
        [SerializeField] private Image icon;
        [SerializeField] private TextMeshProUGUI cooldownText;
        [SerializeField] private TextMeshProUGUI assignedKeyText;
        
        private float _cooldown = 0f;

        private void Start()
        {
            cooldownFillerImage.fillAmount = 0;
            cooldownText.enabled = false;
        }
        
        public void OnSkillUsed(float cooldown)
        {
            _cooldown = cooldown;

            if (Math.Abs(_cooldown) < 0.01)
            {
                return;
            }
            
            StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            cooldownFillerImage.fillAmount = 0f;
            cooldownText.text = FormatCooldownText(_cooldown);
            cooldownText.enabled = true;
            yield return new WaitForEndOfFrame();
            
            var elapsedTime = Time.deltaTime;
            while (elapsedTime < _cooldown)
            {
                cooldownFillerImage.fillAmount = 1f - elapsedTime / _cooldown;
                elapsedTime += Time.deltaTime;

                cooldownText.text = FormatCooldownText(_cooldown - elapsedTime);

                yield return new WaitForEndOfFrame();
            }

            cooldownFillerImage.fillAmount = 0f;
            cooldownText.enabled = false;
        }

        private string FormatCooldownText(float remainingTime)
        {
            if (remainingTime > 60f)
            {
                return $"{remainingTime / 60:F0}:{Mathf.CeilToInt(remainingTime%60):D2}";
            }

            if (remainingTime >= 2f)
            {
                return Mathf.Ceil(remainingTime).ToString("F0");
            }

            if (remainingTime >= 1f)
            {
                return remainingTime.ToString("F1");
            }                
         
            return remainingTime.ToString("F2");
        }

        public void SetAssignedKey(KeyBind keyBind)
        {
            assignedKeyText.text = keyBind.primary.ToString().Replace("Alpha", "");
        }
    }
}