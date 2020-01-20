using UnityEngine;
using UnityEngine.EventSystems;

namespace GearConfigurator
{
    public class MouseOverObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private MouseOverData _data;

        public void SetData(MouseOverData data)
        {
            _data = data;
        }
    
        public void OnPointerEnter(PointerEventData eventData)
        {
            MouseOverDrawer.SetMouseOver(_data);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            MouseOverDrawer.ClearMouseOver();
        }
    }
}
