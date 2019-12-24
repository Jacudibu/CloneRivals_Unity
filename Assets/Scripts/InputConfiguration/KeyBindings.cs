﻿using System;
using UnityEngine;

namespace InputConfiguration
{
    public static class KeyBindings
    {
        [KeyBindAttribute("Strafe/Roll Left")] public static readonly Keybind StrafeLeft = new Keybind(KeyCode.A);
        [KeyBindAttribute("Strafe/Roll Right")] public static readonly Keybind StrafeRight = new Keybind(KeyCode.D);
        [KeyBindAttribute("Boost")] public static readonly Keybind Boost = new Keybind(KeyCode.Space);
        [KeyBindAttribute("Brake")] public static readonly Keybind Brake = new Keybind(KeyCode.S);
        [KeyBindAttribute("Fire Guns")] public static readonly Keybind PrimaryWeapon = new Keybind(KeyCode.Mouse0);
        [KeyBindAttribute("Fire Missiles")] public static readonly Keybind FireMissile = new Keybind(KeyCode.Mouse1);
        [KeyBindAttribute("Cycle next Target")] public static readonly Keybind NextTarget = new Keybind(KeyCode.LeftShift);
        [KeyBindAttribute("Rear Camera")] public static readonly Keybind RearCamera = new Keybind(KeyCode.R);

        [KeyBindAttribute("Hotbar 1")] public static readonly Keybind Hotbar1 = new Keybind(KeyCode.Alpha1);
        [KeyBindAttribute("Hotbar 2")] public static readonly Keybind Hotbar2 = new Keybind(KeyCode.Alpha2);
        [KeyBindAttribute("Hotbar 3")] public static readonly Keybind Hotbar3 = new Keybind(KeyCode.Alpha3);
        [KeyBindAttribute("Hotbar 4")] public static readonly Keybind Hotbar4 = new Keybind(KeyCode.Alpha4);
        [KeyBindAttribute("Hotbar 5")] public static readonly Keybind Hotbar5 = new Keybind(KeyCode.Alpha5);
        [KeyBindAttribute("Hotbar 6")] public static readonly Keybind Hotbar6 = new Keybind(KeyCode.Alpha6);
        [KeyBindAttribute("Hotbar 7")] public static readonly Keybind Hotbar7 = new Keybind(KeyCode.Alpha7);
        [KeyBindAttribute("Hotbar 8")] public static readonly Keybind Hotbar8 = new Keybind(KeyCode.Alpha8);
        [KeyBindAttribute("Hotbar 9")] public static readonly Keybind Hotbar9 = new Keybind(KeyCode.Alpha9);
        [KeyBindAttribute("Hotbar 0")] public static readonly Keybind Hotbar0 = new Keybind(KeyCode.Alpha0);

        public static readonly Func<bool>[] IsHotbarKeyDown =
        {
            () => Hotbar1.IsDown(),
            () => Hotbar2.IsDown(),
            () => Hotbar3.IsDown(),
            () => Hotbar4.IsDown(),
            () => Hotbar5.IsDown(),
            () => Hotbar6.IsDown(),
            () => Hotbar7.IsDown(),
            () => Hotbar8.IsDown(),
            () => Hotbar9.IsDown(),
            () => Hotbar0.IsDown(),
        };
    }
}
