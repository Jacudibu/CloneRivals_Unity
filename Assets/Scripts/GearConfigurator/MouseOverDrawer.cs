using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class MouseOverDrawer : MonoBehaviour
    {
        private static TextMeshProUGUI _title;
        private static TextMeshProUGUI _text;
        private static GameObject _background;
        
        private void Awake()
        {
            var textChildren = GetComponentsInChildren<TextMeshProUGUI>();

            _title = textChildren[0];
            _text = textChildren[1];
            
            _background = GetComponentInChildren<Image>().gameObject;
            ClearMouseOver();
        }
        
        private void Update()
        {
            transform.position = Input.mousePosition;
        }

        public static void ClearMouseOver()
        {
            _title.text = "";
            _text.text = "";
            _background.SetActive(false);
        }
        
        public static void SetMouseOver(MouseOverData data)
        {
            _title.text = data.title;
            _text.text = data.text;
            _background.SetActive(true);
        }
    }
}
