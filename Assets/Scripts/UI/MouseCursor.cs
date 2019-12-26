using UnityEngine;

namespace UI
{
    public class MouseCursor : MonoBehaviour
    {
        [SerializeField] private float cursorRotationOffset = -90;
        private Transform _transform;
        private Vector3 _screenSize;
    
    
        private void Start()
        {
            _transform = transform;
            OnEnable();
        }
    
        private void OnEnable()
        {
            _screenSize = new Vector3(Screen.width / 2f, Screen.height / 2f, 0);
            Cursor.visible = false;
        }

        private void OnDisable()
        {
            Cursor.visible = true;
        }

        void Update()
        {
            var mousePosition = Input.mousePosition - _screenSize;
        
            var angle = Mathf.Atan2(mousePosition.y, mousePosition.x) * Mathf.Rad2Deg;
            _transform.rotation = Quaternion.Euler(0, 0, angle + cursorRotationOffset);
            _transform.position = Input.mousePosition;
        }
    }
}
