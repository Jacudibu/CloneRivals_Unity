using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class BoostSlider : MonoBehaviour
    {
        private Slider _slider;
        [SerializeField] private TextMeshProUGUI valueText;
        [SerializeField] private TextMeshProUGUI effectText;

        [SerializeField] private int speedIncrements = 10;
        [SerializeField] private int timeIncrements = 1;
        
        public int BoostSpeed { get; private set; }
        public int BoostTime { get; private set; }
        
        private void Start()
        {
            _slider = GetComponentInChildren<Slider>();
            OnValueChanged();
        }
        
        public void OnValueChanged()
        {
            var value = (int) _slider.value;

            valueText.text = value.ToString();

            if (value == 0)
            {
                effectText.text = "Default behaviour";
                
                BoostSpeed = 0;
                BoostTime = 0;
                
                return;
            }

            BoostSpeed = value * speedIncrements;
            BoostTime = -value * timeIncrements;

            effectText.text = $"{BoostSpeed:+0;-0} boost speed\n" +
                              $"{BoostTime:+0;-0} boost time";
        }
    }
}