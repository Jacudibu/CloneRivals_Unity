using UnityEngine;

namespace InputConfiguration
{
    public class Keybind
    {
        public KeyCode primary;
        public KeyCode secondary;

        public Keybind(KeyCode primary) : this(primary, primary) { }
        
        public Keybind(KeyCode primary, KeyCode secondary)
        {
            this.primary = primary;
            this.secondary = secondary;
        }

        public bool IsPressed() => Input.GetKey(primary) || Input.GetKey(secondary);
        public bool IsDown() => Input.GetKeyDown(primary) || Input.GetKeyDown(secondary);
    }
}