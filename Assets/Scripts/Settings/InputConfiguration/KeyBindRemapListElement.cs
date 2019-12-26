using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace Settings.InputConfiguration
{
    public class KeyBindRemapListElement : MonoBehaviour
    {
        [SerializeField] private Text text;
        [SerializeField] private Button primary;
        [SerializeField] private Button secondary;

        private Text _primaryText;
        private Text _secondaryText;

        private KeyBind _keyBind;
        
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
            _keyBind = fieldInfo.GetValue(0) as KeyBind;
            
            primary.onClick.AddListener(delegate { KeyBindRemapParent.instance.InitiatePrimaryKeyRemap(remapItemProxy); });
            secondary.onClick.AddListener(delegate { KeyBindRemapParent.instance.InitiateSecondaryKeyRemap(remapItemProxy); });
            
            SetButtonText();
        }

        private void SetButtonText()
        {
            _primaryText.text = _keyBind.primary == null 
                ? "---"
                : _keyBind.primary.ToString();


            _secondaryText.text = _keyBind.secondary == null 
                ? "---" 
                : _keyBind.secondary.ToString();
        }

        public void SetPrimaryKey(KeyCode keyCode)
        {
            _keyBind.primary = keyCode;
            SetButtonText();
        }

        public void SetSecondaryKey(KeyCode keyCode)
        {
            _keyBind.secondary = keyCode;
            SetButtonText();
        }
    }
}