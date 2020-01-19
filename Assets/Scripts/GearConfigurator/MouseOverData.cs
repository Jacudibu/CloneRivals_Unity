using UnityEngine;
using UnityEngine.EventSystems;

namespace GearConfigurator
{
    public class MouseOverData : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        private string _data;

        public void SetData(string data)
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
