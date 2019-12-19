using System;
using UnityEngine;

namespace AI
{
    [RequireComponent(typeof(WeaponController))]
    [RequireComponent(typeof(TargetManager))]
    [RequireComponent(typeof(Engine))]
    public class AIController : MonoBehaviour
    {
        private Engine _engine;
        private WeaponController _weaponController;
        private TargetManager _targetManager;
    
        void Start()
        {
            _engine = GetComponent<Engine>();
            _weaponController = GetComponent<WeaponController>();
            _targetManager = GetComponent<TargetManager>();
        }

        void Update()
        {
            if (_targetManager.Target == null || _targetManager.Target.GetComponent<PlayerController>() == null)
            {
                _targetManager.SearchForTarget();

                if (_targetManager.Target == null)
                {
                    return;
                }
            }

            transform.LookAt(_targetManager.Target.transform);

            if (_weaponController.IsMissileLockable)
            {
                _weaponController.FireMissile();
            }
        }
    }
}
