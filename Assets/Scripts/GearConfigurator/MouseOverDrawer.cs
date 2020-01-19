using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class MouseOverDrawer : MonoBehaviour
    {
        private static TextMeshProUGUI _text;
        private static GameObject _background;
        
        private void Start()
        {
            _text = GetComponentInChildren<TextMeshProUGUI>();
            _background = GetComponentInChildren<Image>().gameObject;
            ClearMouseOver();
        }
        
        private void Update()
        {
            transform.position = Input.mousePosition;
        }

        public static void ClearMouseOver()
        {
            _text.text = "";
            _background.SetActive(false);
        }
        
        public static void SetMouseOver(string text)
        {
            _text.text = text;
            _background.SetActive(true);
        }
    }
}
