using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(Engine))]
    public class AIController : MonoBehaviour
    {
        private Engine _engine;
        private WeaponController _weaponController;
    
        void Start()
        {
            _engine = GetComponent<Engine>();
            _weaponController = GetComponent<WeaponController>();
        }

        void Update()
        {
            if (_weaponController.IsMissileLockable)
            {
                _weaponController.FireMissile();
            }
        }
    }
}
