using UnityEngine;

namespace InputConfiguration
{
    public static class KeyBindings
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

        public static KeyCode FireMissile { get; private set; } = KeyCode.Mouse1;
        public static KeyCode FireMissile2 { get; private set; } = FireMissile;

        public static KeyCode NextTarget { get; private set; } = KeyCode.LeftShift;
        public static KeyCode NextTarget2 { get; private set; } = NextTarget;

        public static KeyCode RearCamera { get; private set; } = KeyCode.R;
        public static KeyCode RearCamera2 { get; private set; } = RearCamera;
        
        
        
        public static bool IsStrafeLeft() => Input.GetKey(StrafeLeft) || Input.GetKey(StrafeLeft2);
        public static bool IsStrafeLeftDown() => Input.GetKeyDown(StrafeLeft) || Input.GetKeyDown(StrafeLeft2);

        public static bool IsStrafeRight() => Input.GetKey(StrafeRight) || Input.GetKey(StrafeRight2);
        public static bool IsStrafeRightDown() => Input.GetKeyDown(StrafeRight) || Input.GetKeyDown(StrafeRight2);

        public static bool IsBoost() => Input.GetKey(Boost) || Input.GetKey(Boost2);
        public static bool IsBoostDown() => Input.GetKeyDown(Boost) || Input.GetKeyDown(Boost2);
        
        public static bool IsBrake() => Input.GetKey(Brake) || Input.GetKey(Brake2);
        public static bool IsBrakeDown() => Input.GetKeyDown(Brake) || Input.GetKeyDown(Brake2);
        
        public static bool IsPrimary() => Input.GetKey(PrimaryWeapon) || Input.GetKey(PrimaryWeapon2);
        public static bool IsPrimaryDown() => Input.GetKeyDown(PrimaryWeapon) || Input.GetKeyDown(PrimaryWeapon2);
        
        public static bool IsFireMissile() => Input.GetKey(FireMissile) || Input.GetKey(FireMissile2);
        public static bool IsFireMissileDown() => Input.GetKeyDown(FireMissile) || Input.GetKeyDown(FireMissile2);
        
        public static bool IsNextTarget() => Input.GetKey(NextTarget) || Input.GetKey(NextTarget2);
        public static bool IsNextTargetDown() => Input.GetKeyDown(NextTarget) || Input.GetKeyDown(NextTarget2);

        public static bool IsRearCamera() => Input.GetKey(RearCamera) || Input.GetKey(RearCamera2);
        public static bool IsRearCameraDown() => Input.GetKeyDown(RearCamera) || Input.GetKeyDown(RearCamera2);
    }
}
