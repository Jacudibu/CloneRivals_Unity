using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace InputConfiguration
{
    public class InputConfigurator : SingletonBehaviour<InputConfigurator>
    {
        [SerializeField] private GameObject remapPopup;
        [SerializeField] private Text debugHelper;

        public static bool IsRemapping { get; private set; }
        
        private TextMeshProUGUI _remapText;
        private InputRemapItem[] _remapItems;

        public void InitiateKeyRemap(InputRemapItem inputRemapItem)
        {
            StartCoroutine( RemappingCoroutine(inputRemapItem,
                x => inputRemapItem.KeyCodeProperty.SetValue(instance, x)));
        }
    
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
            _remapText = remapPopup.GetComponentInChildren<TextMeshProUGUI>();
            remapPopup.SetActive(false);
        
            var childButtons = GetComponentsInChildren<Button>();
            _remapItems = childButtons.Select(x => new InputRemapItem(x.transform.parent.gameObject)).ToArray();
        }

        private void Update()
        {
            debugHelper.text =
                $"Action 1: Strafe Left - {InputSettings.StrafeLeft.ToString()} - {Input.GetKey(InputSettings.StrafeLeft)}\n" +
                $"Action 2: Strafe Right - {InputSettings.StrafeRight.ToString()} - {Input.GetKey(InputSettings.StrafeRight)}\n" +
                $"Action 3: Boost - {InputSettings.Boost.ToString()} - {Input.GetKey(InputSettings.Boost)}";
        }
    
        private IEnumerator RemappingCoroutine(InputRemapItem inputRemapItem, Action<KeyCode> action)
        {
            IsRemapping = true;
            SetButtonInteractivity(false);

            remapPopup.SetActive(true);
            _remapText.text = $"Press new key for\n{inputRemapItem.ActionName}\n(or ESC to cancel)";

            KeyCode? newKey;
            while(true)
            {
                if (Input.GetKey(KeyCode.Escape))
                {
                    yield break;
                }
            
                newKey = GetPressedKey();
                if (newKey != null)
                {
                    break;
                }
            
                yield return new WaitForEndOfFrame();
            }

            action.Invoke((KeyCode) newKey);
            inputRemapItem.SetButtonText();
            remapPopup.SetActive(false);

            SetButtonInteractivity(true);
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