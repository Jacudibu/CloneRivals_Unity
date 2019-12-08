using UnityEngine;
using Utility;

namespace Effects
{
    public class EffectCollection : MonoBehaviour
    {
        [SerializeField] private GameObject shipCrashEffect;
        [SerializeField] private GameObject shipKillEffect;
        [SerializeField] private GameObject smallFire;

        private static EffectCollection _instance;

        private void Awake()
        {
            _instance = this;
        }

        public static GameObject GetShipCrashEffect()
        {
            return _instance.shipCrashEffect;
        }

        public static GameObject GetShipKillEffect()
        {
            return _instance.shipKillEffect;
        }

        public static GameObject GetSmallFire()
        {
            return _instance.smallFire;
        }
    }
}