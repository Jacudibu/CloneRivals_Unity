using UnityEngine;

namespace InputConfiguration
{
    public class Keybind
    {
        public KeyCode? primary;
        public KeyCode? secondary;

        public Keybind(KeyCode? primary = null, KeyCode? secondary = null)
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        public bool IsPressed() => primary != null && Input.GetKey((KeyCode) primary) || 
                                   secondary != null && Input.GetKey((KeyCode) secondary);
        
        public bool IsDown() => primary != null && Input.GetKeyDown((KeyCode) primary) ||
                                secondary != null && Input.GetKeyDown((KeyCode) secondary);
    }
}