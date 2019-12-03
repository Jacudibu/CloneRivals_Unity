using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public List<GameObject> thingsTargetingMe;
    public GameObject target;
    public GameObject currentTargetIndicator;
    public GameObject lockOnReticle;

    public GameObject targetArrow;
    public GameObject targetMeArrow;
    private List<GameObject> _thingsTargetingMeArrows;

    public float radius = 100;
    public float arrowRotationOffset = -90;
    private Camera _camera;
    private Plane[] _cameraFrustumPlanes;
    private WeaponController _weaponController;


    private void Start()
    {
        _camera = Camera.main;
        _weaponController = GetComponent<WeaponController>();
        _thingsTargetingMeArrows = new List<GameObject>
        {
            targetMeArrow
        };
        
        InitializeOrDisableArrows();
    }

    private void LateUpdate()
    {
        _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);

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
                
                if (_weaponController.SecondaryLockable)
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

        for (var i = 0; i < thingsTargetingMe.Count; i++)
        {
            var thing = thingsTargetingMe[i];
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

    private void InitializeOrDisableArrows()
    {
        if (_thingsTargetingMeArrows.Count < thingsTargetingMe.Count)
        {
            var original = _thingsTargetingMeArrows[0];

            while (_thingsTargetingMeArrows.Count < thingsTargetingMe.Count)
            {
                var arrow = Instantiate(original, original.transform.parent, true);
                _thingsTargetingMeArrows.Add(arrow);
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
        var relativePos = transform.InverseTransformPoint(position);
        var normalizedRelativePos = Vector3.Normalize(new Vector3(relativePos.x, relativePos.y, 0));
        arrowTransform.position = normalizedRelativePos * radius + new Vector3(Screen.width * 0.5f, Screen.height * 0.5f, 0);
            
        var angle = Mathf.Atan2(normalizedRelativePos.y, normalizedRelativePos.x) * Mathf.Rad2Deg;
        arrowTransform.rotation = Quaternion.Euler(0, 0, angle + arrowRotationOffset);
    }

}
