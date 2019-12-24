using System;
using System.Linq;
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
            var keyBindAttribute = fieldInfo.GetCustomAttribute<KeyBindAttribute>();
            if (keyBindAttribute == null)
            {
                throw new Exception("Unable to find a keyBindAttribute on keyBind for " + fieldInfo.Name);
            }

            name = keyBindAttribute.name;
            text.text = keyBindAttribute.name;
            
            _primaryText = primary.GetComponentInChildren<Text>();
            _secondaryText = secondary.GetComponentInChildren<Text>();
            
            var remapItemProxy = this;
            _keybind = fieldInfo.GetValue(0) as Keybind;
            
            primary.onClick.AddListener(delegate { KeybindRemapParent.instance.InitiatePrimaryKeyRemap(remapItemProxy); });
            secondary.onClick.AddListener(delegate { KeybindRemapParent.instance.InitiateSecondaryKeyRemap(remapItemProxy); });
            
            SetButtonText();
        }

        private void SetButtonText()
        {
            _primaryText.text = _keybind.primary == null 
                ? "---"
                : _keybind.primary.ToString();


            _secondaryText.text = _keybind.secondary == null 
                ? "---" 
                : _keybind.secondary.ToString();
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