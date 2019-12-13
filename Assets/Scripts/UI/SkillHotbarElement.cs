using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class SkillHotbarElement : MonoBehaviour
    {
        [SerializeField] private Image cooldownFillerImage;
        [SerializeField] private Image icon;
        private float _lastTimeUsed = 0f;
        private float _cooldown = 0f;

        private void Start()
        {
            cooldownFillerImage.fillAmount = 0;
        }
        
        public void OnSkillUsed(float cooldown)
        {
            _lastTimeUsed = Time.time;
            _cooldown = cooldown;

            if (Math.Abs(_cooldown) < 0.01)
            {
                return;
            }
            
            StartCoroutine(CooldownCoroutine());
        }

        private IEnumerator CooldownCoroutine()
        {
            yield return new WaitForEndOfFrame();
            
            var elapsedTime = Time.deltaTime;
            while (elapsedTime < _cooldown)
            {
                cooldownFillerImage.fillAmount = 1f - elapsedTime / _cooldown;
                elapsedTime += Time.deltaTime;
                
                yield return new WaitForEndOfFrame();
            }

            cooldownFillerImage.fillAmount = 0f;
        }
        
    }
}