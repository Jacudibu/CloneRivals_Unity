using System;
using System.Collections;
using System.Linq;
using System.Reflection;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
    private TextMeshProUGUI _remapText;
    
    
    private Button[] _childButtons;
    private InputRemapItem[] remapItems;

    [SerializeField] private Text debugHelper;
    
    [Serializable]
    public struct InputRemapItem    
    {
        public string actionName;
        public PropertyInfo keyCodeProperty;

        private Button button;
        private Text buttonText;
        
        public InputRemapItem(GameObject item)
        {
            actionName = item.name.Replace(" ", "");
            
            button = item.GetComponentInChildren<Button>();
            buttonText = button.GetComponentInChildren<Text>();
            
            keyCodeProperty = instance
                .GetType()
                .GetProperty(actionName);
            
            if (keyCodeProperty == null)
            {
                throw new Exception($"Unable to find KeyCode named {actionName} in inputManager. Check if the Child Names match up!");
            }
            
            var remapItemProxy = this;
            button.onClick.AddListener(delegate {instance.InitiateKeyRemap(remapItemProxy); });
        }
        
        public void SetButtonText()
        {
            buttonText.text = keyCodeProperty.GetValue(0).ToString();
        }
    }

    private void InitiateKeyRemap(InputRemapItem inputRemapItem)
    {
        StartCoroutine( RemappingCoroutine(inputRemapItem, x => inputRemapItem.keyCodeProperty.SetValue(instance, x)));
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
        
        _childButtons = GetComponentsInChildren<Button>();
        remapItems = _childButtons.Select(x => new InputRemapItem(x.transform.parent.gameObject)).ToArray();
        
        foreach (var remapItem in remapItems)
        {
            remapItem.SetButtonText();
        }
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
        _remapText.text = $"Press new key for\n{inputRemapItem.actionName}\n(or ESC to cancel)";

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
        foreach (var button in _childButtons)
        {
            button.interactable = state;
        }
    }
}
