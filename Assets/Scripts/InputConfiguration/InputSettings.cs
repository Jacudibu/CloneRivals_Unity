using UnityEngine;

namespace InputConfiguration
{
    public static class InputSettings
    {
        public static KeyCode StrafeLeft { get; private set; } = KeyCode.A;
        public static KeyCode StrafeLeft2 { get; private set; } = StrafeLeft;
    
        public static KeyCode StrafeRight { get; private set; } = KeyCode.D;
        public static KeyCode StrafeRight2 { get; private set; } = StrafeRight;
    
        public static KeyCode Boost { get; private set; } = KeyCode.Space;
        public static KeyCode Boost2 { get; private set; } = Boost;
    
        public static KeyCode Brake { get; private set; } = KeyCode.S;
        public static KeyCode Brake2 { get; private set; } = Brake;
    
        public static KeyCode PrimaryWeapon { get; private set; } = KeyCode.Mouse0;
        public static KeyCode PrimaryWeapon2 { get; private set; } = PrimaryWeapon;

        public static KeyCode SecondaryWeapon { get; private set; } = KeyCode.Mouse1;
        public static KeyCode SecondaryWeapon2 { get; private set; } = SecondaryWeapon;

        public static KeyCode NextTarget { get; private set; } = KeyCode.LeftShift;
        public static KeyCode NextTarget2 { get; private set; } = NextTarget;

        public static KeyCode ViewBack { get; private set; } = KeyCode.R;
        public static KeyCode ViewBack2 { get; private set; } = ViewBack;
    }
}
