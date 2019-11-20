using System;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : SingletonBehaviour<InputManager>
{
    [SerializeField] private GameObject remapPopup;
    private TextMeshProUGUI _remapText;
    private Button[] _childButtons;

    [SerializeField] private RemapItem remapItem1;
    [SerializeField] private RemapItem remapItem2;
    [SerializeField] private RemapItem remapItem3;

    [Serializable]
    public struct RemapItem    
    {
        public string actionName;
        public GameObject menuItem;

        public void SetButtonText(string text)
        {
            var button = menuItem.GetComponentInChildren<Button>();
            var buttonText = button.GetComponentInChildren<Text>();
            buttonText.text = text;
        }
    }

    private static readonly KeyCode[] IgnoredKeys =
    {
        KeyCode.Escape,

        KeyCode.LeftControl,
        KeyCode.RightControl,
        
        KeyCode.LeftAlt,
        KeyCode.RightAlt,
        
        KeyCode.LeftShift,
        KeyCode.RightShift,
        
        KeyCode.Mouse0,
        KeyCode.Mouse1,
    };
    
    private static readonly KeyCode[] ValidKeys = Enum.GetValues(typeof(KeyCode))
        .Cast<KeyCode>()
        .Except(IgnoredKeys.AsEnumerable())
        .ToArray();
    
    public static KeyCode ActionKey1 { get; private set; } = KeyCode.A;
    public static KeyCode ActionKey2 { get; private set; } = KeyCode.S;
    public static KeyCode ActionKey3 { get; private set; } = KeyCode.D;

    private void Awake()
    {
        _remapText = remapPopup.GetComponentInChildren<TextMeshProUGUI>();
        remapPopup.SetActive(false);

        remapItem1.SetButtonText(ActionKey1.ToString());
        remapItem2.SetButtonText(ActionKey2.ToString());
        remapItem3.SetButtonText(ActionKey3.ToString());

        _childButtons = GetComponentsInChildren<Button>();
    }

    public void RemapTestKey1()
    {
        StartCoroutine( RemappingCoroutine(remapItem1, x => ActionKey1 = x));
    }
    
    public void RemapTestKey2()
    {
        StartCoroutine(RemappingCoroutine(remapItem2, x => ActionKey2 = x));
    }
    
    public void RemapTestKey3()
    {
        StartCoroutine(RemappingCoroutine(remapItem3, x => ActionKey3 = x));
    }

    private IEnumerator RemappingCoroutine(RemapItem remapItem, Func<KeyCode, KeyCode> func)
    {
        SetButtonInteractivity(false);

        remapPopup.SetActive(true);
        _remapText.text = $"Press new key for\n{remapItem.actionName}\n(or ESC to cancel)";

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
        remapItem.SetButtonText(newKey.ToString());
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
