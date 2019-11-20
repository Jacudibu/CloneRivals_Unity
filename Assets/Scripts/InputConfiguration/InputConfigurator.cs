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
        public static KeyCode StrafeLeft { get; private set; } = KeyCode.A;
        public static KeyCode StrafeLeft2 { get; private set; } = StrafeLeft;
    
        public static KeyCode StrafeRight { get; private set; } = KeyCode.D;
        public static KeyCode StrafeRight2 { get; private set; } = StrafeRight;
    
        public static KeyCode Boost { get; private set; } = KeyCode.Space;
        public static KeyCode Boost2 { get; private set; } = Boost;
    
        public static KeyCode Brake { get; private set; } = KeyCode.S;
        public static KeyCode Brake2 { get; private set; } = Brake;
    
        public static KeyCode PrimaryWeapon { get; private set; } = KeyCode.Mouse0;
        public static KeyCode PrimaryWeapon2 { get; private set; } = PrimaryWeapon;

        public static KeyCode SecondaryWeapon { get; private set; } = KeyCode.Mouse1;
        public static KeyCode SecondaryWeapon2 { get; private set; } = SecondaryWeapon;

        public static KeyCode NextTarget { get; private set; } = KeyCode.LeftShift;
        public static KeyCode NextTarget2 { get; private set; } = NextTarget;

        public static KeyCode ViewBack { get; private set; } = KeyCode.R;
        public static KeyCode ViewBack2 { get; private set; } = ViewBack;

        [SerializeField] private GameObject remapPopup;
        [SerializeField] private Text debugHelper;
    
        private TextMeshProUGUI _remapText;
        private InputRemapItem[] _remapItems;
    
        public void InitiateKeyRemap(InputRemapItem inputRemapItem)
        {
            StartCoroutine( RemappingCoroutine(inputRemapItem, x => inputRemapItem.KeyCodeProperty.SetValue(instance, x)));
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
                $"Action 1: Strafe Left - {StrafeLeft.ToString()} - {Input.GetKey(StrafeLeft)}\n" +
                $"Action 2: Strafe Right - {StrafeRight.ToString()} - {Input.GetKey(StrafeRight)}\n" +
                $"Action 3: Boost - {Boost.ToString()} - {Input.GetKey(Boost)}";
        }
    
        private IEnumerator RemappingCoroutine(InputRemapItem inputRemapItem, Action<KeyCode> func)
        {
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

            func.Invoke((KeyCode) newKey);
            inputRemapItem.SetButtonText();
            remapPopup.SetActive(false);

            SetButtonInteractivity(true);
        }

        private static KeyCode? GetPressedKey()
        {
            if (Input.anyKey)
            {
                foreach (var key in ValidKeys)
                {
                    if (Input.GetKey(key))
                    {
                        return key;
                    }
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