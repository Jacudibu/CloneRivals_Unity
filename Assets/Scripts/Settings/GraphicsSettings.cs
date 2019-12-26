using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Settings
{
    public class GraphicsSettings : MonoBehaviour
    {
        [SerializeField] private Dropdown resolutionDropdown;
        [SerializeField] private Toggle fullscreenToggle;
        
        private void Start()
        {
            var invertedResolutions = Screen.resolutions.Reverse().ToArray();
            for (var i = 0; i < Screen.resolutions.Length; i++)
            {
                var resolution = invertedResolutions[i];
                resolutionDropdown.options.Add(new Dropdown.OptionData(resolution.ToString().Split('@')[0]));
                
                if (resolution.width == Screen.currentResolution.width && resolution.height == Screen.currentResolution.height)
                {
                    resolutionDropdown.SetValueWithoutNotify(i);
                }
            }
        }

        public void OnResolutionOptionChanged(int index)
        {
            Debug.Log("Selected resolution option " + index);
            var resolution = Screen.resolutions[index];
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(resolution.width, resolution.height, fullscreenToggle.isOn);
        }

        public void OnFullscreenToggleOptionChanged(bool value)
        {
            Screen.fullScreenMode = FullScreenMode.FullScreenWindow;
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, value);
        }
    }
}