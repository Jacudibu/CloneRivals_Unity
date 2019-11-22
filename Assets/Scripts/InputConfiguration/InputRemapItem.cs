using System;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace InputConfiguration
{
    public struct InputRemapItem    
    {
        public readonly string ActionName;
        public readonly PropertyInfo KeyCodeProperty;
        public readonly Button Button;
    
        private readonly Text _buttonText;
        
        public InputRemapItem(GameObject item)
        {
            ActionName = item.name.Replace(" ", "");
            
            Button = item.GetComponentInChildren<Button>();
            _buttonText = Button.GetComponentInChildren<Text>();
            
            KeyCodeProperty = typeof(KeyBindings).GetProperty(ActionName);
            
            if (KeyCodeProperty == null)
            {
                throw new Exception($"Unable to find KeyCode named {ActionName} in inputManager. Check if the Child Names all match up!");
            }
            
            var remapItemProxy = this;
            Button.onClick.AddListener(delegate {InputConfigurator.instance.InitiateKeyRemap(remapItemProxy); });
            
            SetButtonText();
        }
        
        public void SetButtonText()
        {
            _buttonText.text = KeyCodeProperty.GetValue(0).ToString();
        }
    }
}