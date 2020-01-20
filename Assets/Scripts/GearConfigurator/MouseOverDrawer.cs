using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace GearConfigurator
{
    public class MouseOverDrawer : MonoBehaviour
    {
        private static TextMeshProUGUI _title;
        private static TextMeshProUGUI _text;
        private static GameObject _imageBar;
        private static GameObject _imagePrefab;
        private static GameObject _background;


        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI text;
        [SerializeField] private GameObject imageBar;
        [SerializeField] private GameObject imagePrefab;
        [SerializeField] private GameObject background;


        private void Awake()
        {
            _title = title;
            _text = text;
            _imageBar = imageBar;
            _imagePrefab = imagePrefab;
            imagePrefab.SetActive(false);
            
            _background = background;
            
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

            for (var i = 1; i < _imageBar.transform.childCount; i++)
            {
                Destroy(_imageBar.transform.GetChild(i).gameObject);
            }
            
            _background.SetActive(false);
        }
        
        public static void SetMouseOver(MouseOverData data)
        {
            _background.SetActive(true);
            
            _title.text = data.title;
            
            foreach (var sprite in data.images)
            {
                var image = Instantiate(_imagePrefab, _imageBar.transform);
                image.GetComponentInChildren<Image>().sprite = sprite;
                image.SetActive(true);
            }
            
            _text.text = data.text;
        }
    }
}
