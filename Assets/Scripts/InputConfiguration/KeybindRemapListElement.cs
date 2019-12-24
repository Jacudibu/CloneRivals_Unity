using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace InputConfiguration
{
    public class KeybindRemapListElement : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Button primary;
        [SerializeField] private Button secondary;

        private Text _primaryText;
        private Text _secondaryText;

        private Keybind _keybind;
        
        public void Initialize(FieldInfo fieldInfo)
        {
            name = fieldInfo.Name;
            text.text = fieldInfo.Name;

            _primaryText = primary.GetComponentInChildren<Text>();
            _secondaryText = secondary.GetComponentInChildren<Text>();
            
            var remapItemProxy = this;
            _keybind = fieldInfo.GetValue(0) as Keybind;
            
            primary.onClick.AddListener(delegate { InputConfigurator.instance.InitiatePrimaryKeyRemap(remapItemProxy); });
            secondary.onClick.AddListener(delegate { InputConfigurator.instance.InitiateSecondaryKeyRemap(remapItemProxy); });
            
            SetButtonText();
        }

        private void SetButtonText()
        {
            _primaryText.text = _keybind.primary.ToString();

            if (_keybind.primary == _keybind.secondary)
            {
                _secondaryText.text = "---";
            }
            else
            {
                _secondaryText.text = _keybind.secondary.ToString();
            }
        }

        public void SetPrimaryKey(KeyCode keyCode)
        {
            _keybind.primary = keyCode;
            SetButtonText();
        }

        public void SetSecondaryKey(KeyCode keyCode)
        {
            _keybind.secondary = keyCode;
            SetButtonText();
        }
    }
}