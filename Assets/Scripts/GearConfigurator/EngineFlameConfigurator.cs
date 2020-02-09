using UnityEngine;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class EngineFlameConfigurator : MonoBehaviour
    {
        [SerializeField] private Button innerFlameButton;
        [SerializeField] private Button outerFlameButton;

        [SerializeField] private GameObject colorPickerParent;
        private CUIColorPicker.CUIColorPicker _colorPicker;

        private enum Flame
        {
            Inner,
            Outer,
        }
        private Flame _currentFlame;
        
        private void Awake()
        {
            _colorPicker = colorPickerParent.GetComponent<CUIColorPicker.CUIColorPicker>();
            _colorPicker.SetOnValueChangeCallback(OnColorSelected);
            CloseColorPicker();
        }
        
        public void OpenColorPickerInnerFlame()
        {
            _currentFlame = Flame.Inner;
            _colorPicker.Color = GearConfiguration.Current.engineFlameConfiguration.innerFlameColor;
            colorPickerParent.SetActive(true);
        }
        
        public void OpenColorPickerOuterFlame()
        {
            _currentFlame = Flame.Outer;
            _colorPicker.Color = GearConfiguration.Current.engineFlameConfiguration.outerFlameColor;
            colorPickerParent.SetActive(true);
        }

        public void CloseColorPicker()
        {
            colorPickerParent.SetActive(false);
        }

        public void OnColorSelected(Color color)
        {
            if (_currentFlame == Flame.Inner)
            {
                innerFlameButton.GetComponent<Image>().color = color;
                GearConfiguration.Current.engineFlameConfiguration.innerFlameColor = color;
            }
            else
            {
                outerFlameButton.GetComponent<Image>().color = color;
                GearConfiguration.Current.engineFlameConfiguration.outerFlameColor = color;
            }
        }
    }
}