using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class TargetingUI : MonoBehaviour
    {
        private TargetManager _targetManager;
        private WeaponController _weaponController;
    
        public GameObject currentTargetIndicator;
        public GameObject lockOnReticle;

        public GameObject directionalReticle1;
        public GameObject directionalReticle2;
        public float directionalReticleDistance1 = 50;
        public float directionalReticleDistance2 = 150;
        
        public GameObject targetArrow;
        public GameObject targetMeArrow;
        private List<GameObject> _thingsTargetingMeArrows;
    
        public float radius = 200;
        public float arrowRotationOffset = -90;
        private Camera _camera;
        private Plane[] _cameraFrustumPlanes;
    
        void Start()
        {
            var player = FindObjectOfType<PlayerController>();
            _targetManager = player.GetComponent<TargetManager>();
            _weaponController = player.GetComponent<WeaponController>();
        
            _camera = Camera.main;
            _thingsTargetingMeArrows = new List<GameObject>
            {
                targetMeArrow
            };

            InitializeOrDisableArrows();
            _targetManager.OnIncomingTargetUpdate += InitializeOrDisableArrows;
        }

        private void InitializeOrDisableArrows()
        {
            if (_thingsTargetingMeArrows.Count < _targetManager.thingsTargetingMe.Count)
            {
                var original = _thingsTargetingMeArrows[0];

                while (_thingsTargetingMeArrows.Count < _targetManager.thingsTargetingMe.Count)
                {
                    var arrow = Instantiate(original, original.transform.parent, true);
                    _thingsTargetingMeArrows.Add(arrow);
                }
            } 
            else if (_thingsTargetingMeArrows.Count > _targetManager.thingsTargetingMe.Count)
            {
                for (var i = _targetManager.thingsTargetingMe.Count; i < _thingsTargetingMeArrows.Count; i++)
                {
                    _thingsTargetingMeArrows[i].SetActive(false);
                }
            }
        }

        private void Update()
        {
            directionalReticle1.transform.position = _camera.WorldToScreenPoint(_weaponController.transform.position + _weaponController.transform.forward * directionalReticleDistance1);
            directionalReticle2.transform.position = _camera.WorldToScreenPoint(_weaponController.transform.position + _weaponController.transform.forward * directionalReticleDistance2);
        }
        
        private void LateUpdate()
        {
            _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);

            var target = _targetManager.Target;
            if (target != null)
            {
                if (!IsObjectInScreenArea(target.transform.position))
                {
                    targetArrow.SetActive(true);
                    lockOnReticle.SetActive(false);
                    currentTargetIndicator.SetActive(false);
                    AdjustArrowTransform(targetArrow.transform, target.transform.position);
                }
                else
                {
                    targetArrow.SetActive(false);
                    currentTargetIndicator.SetActive(true);

                    var targetScreenPosition = _camera.WorldToScreenPoint(target.transform.position);
                    currentTargetIndicator.transform.position = targetScreenPosition;
                
                    if (_weaponController.IsMissileLockable)
                    {
                        lockOnReticle.SetActive(true);
                        lockOnReticle.transform.position = targetScreenPosition;
                    }
                    else
                    {
                        lockOnReticle.SetActive(false);
                    }
                }
            }
            else
            {
                targetArrow.SetActive(false);
                lockOnReticle.SetActive(false);
                currentTargetIndicator.SetActive(false);
            }

            for (var i = 0; i < _targetManager.thingsTargetingMe.Count; i++)
            {
                var thing = _targetManager.thingsTargetingMe[i];
                var arrow = _thingsTargetingMeArrows[i];

                if (!IsObjectInScreenArea(thing.transform.position))
                {
                    arrow.SetActive(true);
                    AdjustArrowTransform(arrow.transform, thing.transform.position);
                }
                else
                {
                    arrow.SetActive(false);
                }
            }
        }

        private bool IsObjectInScreenArea(Vector3 position)
        {
            var bounds = new Bounds(position, Vector3.one);
            return GeometryUtility.TestPlanesAABB(_cameraFrustumPlanes, bounds);
        }

        private void AdjustArrowTransform(Transform arrowTransform, Vector3 position)
        {
            var relativePos = _targetManager.transform.InverseTransformPoint(position);
            var normalizedRelativePos = Vector3.Normalize(new Vector3(relativePos.x, relativePos.y, 0));
            arrowTransform.position = normalizedRelativePos * radius + new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            
            var angle = Mathf.Atan2(normalizedRelativePos.y, normalizedRelativePos.x) * Mathf.Rad2Deg;
            arrowTransform.rotation = Quaternion.Euler(0, 0, angle + arrowRotationOffset);
        }
    }
}
