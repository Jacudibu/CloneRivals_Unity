using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class PlayerStatusUI : MonoBehaviour
    {
        [SerializeField] private Image structureBar;
        [SerializeField] private Image shieldBar;
        [SerializeField] private Image boostBar;
        [SerializeField] private TextMeshProUGUI speedText;
    
        private PlayerController _player;
        private Engine _engine;
    
        private void Start()
        {
            _player = FindObjectOfType<PlayerController>();
            _engine = _player.GetComponent<Engine>();
        
            var targetable = _player.GetComponent<TargetableObject>();
            targetable.OnHealthChanged.AddListener(OnHealthChange);

            boostBar.fillAmount = 1f;
            structureBar.fillAmount = 1f;
            shieldBar.fillAmount = 1f;
        }

        private void LateUpdate()
        {
            boostBar.fillAmount = _player.GetOverheatRatio();
            speedText.text = (_engine.currentSpeed * 10).ToString("F0");
        }
    
        private void OnHealthChange(TargetableObject targetable)
        {
            structureBar.fillAmount = targetable.GetStructurePercentage();
            shieldBar.fillAmount = targetable.GetShieldPercentage();
        }
    }
}
