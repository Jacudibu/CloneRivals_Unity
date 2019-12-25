using System;
using System.IO;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace InputConfiguration
{
    public static class KeyBindings
    {
        [KeyBindAttribute("Strafe/Roll Left")] public static readonly KeyBind StrafeLeft = new KeyBind(KeyCode.A);
        [KeyBindAttribute("Strafe/Roll Right")] public static readonly KeyBind StrafeRight = new KeyBind(KeyCode.D);
        [KeyBindAttribute("Boost")] public static readonly KeyBind Boost = new KeyBind(KeyCode.Space);
        [KeyBindAttribute("Brake")] public static readonly KeyBind Brake = new KeyBind(KeyCode.S);
        [KeyBindAttribute("Fire Guns")] public static readonly KeyBind PrimaryWeapon = new KeyBind(KeyCode.Mouse0);
        [KeyBindAttribute("Fire Missiles")] public static readonly KeyBind FireMissile = new KeyBind(KeyCode.Mouse1);
        [KeyBindAttribute("Cycle next Target")] public static readonly KeyBind NextTarget = new KeyBind(KeyCode.LeftShift);
        [KeyBindAttribute("Rear Camera")] public static readonly KeyBind RearCamera = new KeyBind(KeyCode.R);

        [KeyBindAttribute("Hotbar 1")] public static readonly KeyBind Hotbar1 = new KeyBind(KeyCode.Alpha1);
        [KeyBindAttribute("Hotbar 2")] public static readonly KeyBind Hotbar2 = new KeyBind(KeyCode.Alpha2);
        [KeyBindAttribute("Hotbar 3")] public static readonly KeyBind Hotbar3 = new KeyBind(KeyCode.Alpha3);
        [KeyBindAttribute("Hotbar 4")] public static readonly KeyBind Hotbar4 = new KeyBind(KeyCode.Alpha4);
        [KeyBindAttribute("Hotbar 5")] public static readonly KeyBind Hotbar5 = new KeyBind(KeyCode.Alpha5);
        [KeyBindAttribute("Hotbar 6")] public static readonly KeyBind Hotbar6 = new KeyBind(KeyCode.Alpha6);
        [KeyBindAttribute("Hotbar 7")] public static readonly KeyBind Hotbar7 = new KeyBind(KeyCode.Alpha7);
        [KeyBindAttribute("Hotbar 8")] public static readonly KeyBind Hotbar8 = new KeyBind(KeyCode.Alpha8);
        [KeyBindAttribute("Hotbar 9")] public static readonly KeyBind Hotbar9 = new KeyBind(KeyCode.Alpha9);
        [KeyBindAttribute("Hotbar 0")] public static readonly KeyBind Hotbar0 = new KeyBind(KeyCode.Alpha0);

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


        private const string KeyBindingFileName = "keybindings.txt";
        private static readonly string KeyBindingsFilePath = Application.persistentDataPath + KeyBindingFileName;
        private const string AttributeNameDelimiter = ": ";
        private const string KeyCodeDelimiter = ", ";
        private const string NullKeyCodeIdentifier = "none";
        public static void LoadFromDisk()
        {
            if (!File.Exists(KeyBindingsFilePath))
            {
                SaveToDisk();
                return;
            }
            
            var lines = File.ReadAllLines(KeyBindingsFilePath);
            var nameDelimiter = new[]{AttributeNameDelimiter};
            var keyBindDelimiter = new[]{KeyCodeDelimiter};
            
            var fields = typeof(KeyBindings).GetFields().Where(x => x.FieldType == typeof(KeyBind)).ToArray();
            foreach (var line in lines)
            {
                if (line.Length == 0)
                {
                    continue;
                }
                
                var split = line.Split(nameDelimiter, StringSplitOptions.None);
                var attributeName = split[0];
                var keyBindSplit = split[1].Split(keyBindDelimiter, StringSplitOptions.None);
                var key1 = keyBindSplit[0];
                var key2 = keyBindSplit[1];

                var field = fields.Single(x => x.GetCustomAttribute<KeyBindAttribute>().name == attributeName);
                var keyBind = field.GetValue(0) as KeyBind;
                System.Diagnostics.Debug.Assert(keyBind != null, nameof(keyBind) + " != null");

                if (!key1.Equals(NullKeyCodeIdentifier))
                {
                    keyBind.primary = (KeyCode) Enum.Parse(typeof(KeyCode), key1);
                }
                
                if (!key2.Equals(NullKeyCodeIdentifier))
                {
                    keyBind.secondary = (KeyCode) Enum.Parse(typeof(KeyCode), key2);
                }
            }
        }

        public static void SaveToDisk()
        {
            var fileContent = "";
            var fields = typeof(KeyBindings).GetFields().Where(x => x.FieldType == typeof(KeyBind));
            foreach (var fieldInfo in fields)
            {
                var keyBindAttribute = fieldInfo.GetCustomAttribute<KeyBindAttribute>();
                if (keyBindAttribute == null)
                {
                    throw new Exception("Unable to find a keyBindAttribute on keyBind for " + fieldInfo.Name);
                }

                if (fieldInfo.GetValue(0) is KeyBind keyBind)
                {
                    fileContent += 
                        keyBindAttribute.name + AttributeNameDelimiter + 
                        (keyBind.primary != null ? keyBind.primary.ToString() : NullKeyCodeIdentifier) +
                        KeyCodeDelimiter +
                        (keyBind.secondary != null ? keyBind.secondary.ToString() : NullKeyCodeIdentifier) + 
                        "\n";
                }
            }
            
            File.WriteAllText(KeyBindingsFilePath, fileContent);
        }
    }
}
