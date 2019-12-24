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
    public class InputConfigurator : SingletonBehaviour<InputConfigurator>
    {
        [SerializeField] private GameObject remapPopup;
        [SerializeField] private GameObject keybindListElement;
        
        public static bool IsRemapping { get; private set; }
        
        private TextMeshProUGUI _remapPopupText;
        private InputRemapItem[] _remapItems;

        private static readonly KeyCode[] IgnoredKeys =
        {
            KeyCode.Escape,
        };

        private static readonly KeyCode[] ValidKeys = Enum.GetValues(typeof(KeyCode))
            .Cast<KeyCode>()
            .Except(IgnoredKeys.AsEnumerable())
            .ToArray();

        private void Start()
        {
            _remapPopupText = remapPopup.GetComponentInChildren<TextMeshProUGUI>();
            remapPopup.SetActive(false);
        
            var keybinds = typeof(KeyBindings).GetFields().Where(x => x.FieldType == typeof(Keybind));
            foreach (var fieldInfo in keybinds)
            {
                var elementObject = Instantiate(keybindListElement, transform);
                var elementScript = elementObject.GetComponent<KeybindRemapListElement>();
                elementScript.Initialize(fieldInfo);
            }
            
            var childButtons = GetComponentsInChildren<Button>();
            _remapItems = childButtons.Select(x => new InputRemapItem(x.transform.parent.gameObject)).ToArray();
        }

        public void InitiateKeyRemap(InputRemapItem inputRemapItem)
        {
            StartCoroutine( RemappingCoroutine(inputRemapItem,
                x => inputRemapItem.KeyCodeProperty.SetValue(instance, x)));
        }

        private IEnumerator RemappingCoroutine(InputRemapItem inputRemapItem, Action<KeyCode> action)
        {
            IsRemapping = true;
            var eventSystem = EventSystem.current.gameObject;
            eventSystem.SetActive(false);
            SetButtonInteractivity(false);

            remapPopup.SetActive(true);
            _remapPopupText.text = $"Press new key for\n{inputRemapItem.ActionName}\n(or ESC to cancel)";

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
                inputRemapItem.SetButtonText();
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
            foreach (var remapItem in _remapItems)
            {
                remapItem.Button.interactable = state;
            }
        }
    }
}