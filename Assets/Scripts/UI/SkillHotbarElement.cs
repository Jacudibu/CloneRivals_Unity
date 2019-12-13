using System;
using System.Collections;
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
            cooldownText.text = _cooldown.ToString("F0");
            cooldownText.enabled = true;
            yield return new WaitForEndOfFrame();
            
            var elapsedTime = Time.deltaTime;
            while (elapsedTime < _cooldown)
            {
                cooldownFillerImage.fillAmount = 1f - elapsedTime / _cooldown;
                elapsedTime += Time.deltaTime;

                var remainingTime = _cooldown - elapsedTime;

                if (remainingTime > 60)
                {
                    cooldownText.text = $"{remainingTime / 60:F0}:{Mathf.CeilToInt(remainingTime%60):D2}";
                }
                else if (remainingTime >= 1f)
                {
                    cooldownText.text = Mathf.Ceil(remainingTime).ToString("F0");
                }
                else
                {
                    cooldownText.text = remainingTime.ToString("F2");
                }
                yield return new WaitForEndOfFrame();
            }

            cooldownFillerImage.fillAmount = 0f;
            cooldownText.enabled = false;
        }
        
    }
}