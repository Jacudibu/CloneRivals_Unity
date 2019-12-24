using System;
using UnityEngine;

namespace InputConfiguration
{
    public static class KeyBindings
    {
        public static readonly Keybind StrafeLeft = new Keybind(KeyCode.A);
        public static readonly Keybind StrafeRight = new Keybind(KeyCode.D);
        public static readonly Keybind Boost = new Keybind(KeyCode.Space);
        public static readonly Keybind Brake = new Keybind(KeyCode.S);
        public static readonly Keybind PrimaryWeapon = new Keybind(KeyCode.Mouse0);
        public static readonly Keybind FireMissile = new Keybind(KeyCode.Mouse1);
        public static readonly Keybind NextTarget = new Keybind(KeyCode.LeftShift);
        public static readonly Keybind RearCamera = new Keybind(KeyCode.R);

        public static KeyCode Hotbar1 { get; private set; } = KeyCode.Alpha1;
        public static KeyCode Hotbar12 { get; private set; } = Hotbar1;
        
        public static KeyCode Hotbar2 { get; private set; } = KeyCode.Alpha2;
        public static KeyCode Hotbar22 { get; private set; } = Hotbar2;
        
        public static KeyCode Hotbar3 { get; private set; } = KeyCode.Alpha3;
        public static KeyCode Hotbar32 { get; private set; } = Hotbar3;
        
        public static KeyCode Hotbar4 { get; private set; } = KeyCode.Alpha4;
        public static KeyCode Hotbar42 { get; private set; } = Hotbar4;
        
        public static KeyCode Hotbar5 { get; private set; } = KeyCode.Alpha5;
        public static KeyCode Hotbar52 { get; private set; } = Hotbar5;
        
        public static KeyCode Hotbar6 { get; private set; } = KeyCode.Alpha6;
        public static KeyCode Hotbar62 { get; private set; } = Hotbar6;
        
        public static KeyCode Hotbar7 { get; private set; } = KeyCode.Alpha7;
        public static KeyCode Hotbar72 { get; private set; } = Hotbar7;
        
        public static KeyCode Hotbar8 { get; private set; } = KeyCode.Alpha8;
        public static KeyCode Hotbar82 { get; private set; } = Hotbar8;
        
        public static KeyCode Hotbar9 { get; private set; } = KeyCode.Alpha9;
        public static KeyCode Hotbar92 { get; private set; } = Hotbar9;
        
        public static KeyCode Hotbar0 { get; private set; } = KeyCode.Alpha0;
        public static KeyCode Hotbar02 { get; private set; } = Hotbar0;

        public static readonly Func<bool>[] IsHotbarKeyDown =
        {
            () => Input.GetKeyDown(Hotbar1) || Input.GetKeyDown(Hotbar12),
            () => Input.GetKeyDown(Hotbar2) || Input.GetKeyDown(Hotbar22),
            () => Input.GetKeyDown(Hotbar3) || Input.GetKeyDown(Hotbar32),
            () => Input.GetKeyDown(Hotbar4) || Input.GetKeyDown(Hotbar42),
            () => Input.GetKeyDown(Hotbar5) || Input.GetKeyDown(Hotbar52),
            () => Input.GetKeyDown(Hotbar6) || Input.GetKeyDown(Hotbar62),
            () => Input.GetKeyDown(Hotbar7) || Input.GetKeyDown(Hotbar72),
            () => Input.GetKeyDown(Hotbar8) || Input.GetKeyDown(Hotbar82),
            () => Input.GetKeyDown(Hotbar9) || Input.GetKeyDown(Hotbar92),
            () => Input.GetKeyDown(Hotbar0) || Input.GetKeyDown(Hotbar02),
        };
    }
}
