using UnityEngine;
using UnityEngine.UIElements;

namespace GearConfigurator
{
    public class EngineFlameConfigurator : MonoBehaviour
    {
        [SerializeField] private Button innerFlameButton;
        [SerializeField] private Button outerFlameButton;

        [SerializeField] private GameObject colorPickerParent;
        
        public void OpenColorPicker()
        {
            colorPickerParent.SetActive(true);
        }

        public void CloseColorPicker()
        {
            colorPickerParent.SetActive(false);
        }

        public void OnColorSelection()
        {
            CloseColorPicker();
        }
    }
}