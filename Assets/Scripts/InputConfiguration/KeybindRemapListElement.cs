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

        public void Initialize(FieldInfo fieldInfo)
        {
            name = fieldInfo.Name;
            text.text = fieldInfo.Name;
            
            
        }
    }
}