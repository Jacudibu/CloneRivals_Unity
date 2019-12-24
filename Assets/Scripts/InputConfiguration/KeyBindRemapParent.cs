using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utility;

namespace InputConfiguration
{
    public class KeyBindRemapParent : SingletonBehaviour<KeyBindRemapParent>
    {
        [SerializeField] private GameObject remapPopup;
        [SerializeField] private GameObject keybindListElement;
        
        public static bool IsRemapping { get; private set; }
        
        private TextMeshProUGUI _remapPopupText;

        private static readonly KeyCode[] IgnoredKeys =
        {
            KeyCode.Escape,
        };

        private static readonly KeyCode[] ValidKeys = Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Except(IgnoredKeys.AsEnumerable())
            .ToArray();

        private Button[] _childButtons;
        
        private void Start()
        {
            _remapPopupText = remapPopup.GetComponentInChildren<TextMeshProUGUI>();
            remapPopup.SetActive(false);
        
            var keyBinds = typeof(KeyBindings).GetFields().Where(x => x.FieldType == typeof(KeyBind));
            foreach (var fieldInfo in keyBinds)
            {
                var elementObject = Instantiate(keybindListElement, transform);
                var elementScript = elementObject.GetComponent<KeyBindRemapListElement>();
                elementScript.Initialize(fieldInfo);
            }
            
            _childButtons = GetComponentsInChildren<Button>();
        }

        private IEnumerator RemappingCoroutine(Action<KeyCode> action)
        {
            IsRemapping = true;
            var eventSystem = EventSystem.current.gameObject;
            eventSystem.SetActive(false);
            SetButtonInteractivity(false);

            remapPopup.SetActive(true);

            KeyCode? newKey = null;
            while(true)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    break;
                }
            
                newKey = GetPressedKey();
                if (newKey != null)
                {
                    break;
                }
            
                yield return new WaitForEndOfFrame();
            }

            if (newKey != null)
            {
                action.Invoke((KeyCode) newKey);
            }
            
            remapPopup.SetActive(false);
            SetButtonInteractivity(true);
            eventSystem.SetActive(true);
            IsRemapping = false;
        }

        private static KeyCode? GetPressedKey()
        {
            if (!Input.anyKey)
            {
                return null;
            }
            
            foreach (var key in ValidKeys)
            {
                if (Input.GetKey(key))
                {
                    return key;
                }
            }

            return null;
        }

        private void SetButtonInteractivity(bool state)
        {
            foreach (var button in _childButtons)
            {
                button.interactable = state;
            }
        }

        public void InitiatePrimaryKeyRemap(KeyBindRemapListElement remapItemProxy)
        {
            Debug.Log("Primary remap invoked for " + remapItemProxy.name);
            _remapPopupText.text = $"Press new key for\n{remapItemProxy.name}\n(or ESC to cancel)";
            StartCoroutine( RemappingCoroutine(remapItemProxy.SetPrimaryKey));
        }

        public void InitiateSecondaryKeyRemap(KeyBindRemapListElement remapItemProxy)
        {
            Debug.Log("Secondary remap invoked for " + remapItemProxy.name);
            _remapPopupText.text = $"Press new key for\n{remapItemProxy.name}\n(or ESC to cancel)";
            StartCoroutine( RemappingCoroutine(remapItemProxy.SetSecondaryKey));
        }
    }
}