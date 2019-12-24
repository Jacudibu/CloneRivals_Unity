using System;

namespace InputConfiguration
{
    public class KeyBindAttribute : Attribute
    {
        public readonly string name;
        
        public KeyBindAttribute(string name)
        {
            this.name = name;
        }
    }
}