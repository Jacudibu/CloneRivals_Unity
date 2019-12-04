﻿using System.Collections.Generic;
using System.Linq;
using InputConfiguration;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public List<GameObject> thingsTargetingMe;
    public GameObject Target { get; private set; }
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

    private Transform _shipTransform;
    [SerializeField] private float targetLockRange = 200f;
    [SerializeField] private float targetLockAngle = 25f;
    [SerializeField] private Collider coneCollider;
    
    private void Start()
    {
        _camera = Camera.main;
        _weaponController = GetComponent<WeaponController>();
        _shipTransform = GetComponentInChildren<MeshRenderer>().transform;
        _thingsTargetingMeArrows = new List<GameObject>
        {
            targetMeArrow
        };
        
        InitializeOrDisableArrows();
    }

    private void Update()
    {
        if (!KeyBindings.IsNextTargetDown() && Target != null)
        {
            return;
        }

        var viableTargets = Physics.OverlapSphere(_shipTransform.position, targetLockRange)
            .Where(x => x.GetComponent<TargetableObject>() != null)
            .Where(x => Mathf.Abs(Vector3.Angle(_shipTransform.forward, x.transform.position - _shipTransform.position)) < targetLockAngle)
            .Where(x => x.gameObject != Target)
            .Select(x => x.gameObject);

        Target = viableTargets.FirstOrDefault();
    }

    private void LateUpdate()
    {
        _cameraFrustumPlanes = GeometryUtility.CalculateFrustumPlanes(_camera);

        if (Target != null)
        {
            if (!IsObjectInScreenArea(Target.transform.position))
            {
                targetArrow.SetActive(true);
                lockOnReticle.SetActive(false);
                currentTargetIndicator.SetActive(false);
                AdjustArrowTransform(targetArrow.transform, Target.transform.position);
            }
            else
            {
                targetArrow.SetActive(false);
                currentTargetIndicator.SetActive(true);

                var targetScreenPosition = _camera.WorldToScreenPoint(Target.transform.position);
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
        else
        {
            lockOnReticle.SetActive(false);
            currentTargetIndicator.SetActive(false);
        }

        for (var i = 0; i < thingsTargetingMe.Count; i++)
        {
            var thing = thingsTargetingMe[i];
            var arrow = _thingsTargetingMeArrows[i];

            if (thing == null)
            {
                //TODO: Listen to OnTargetableDestroyed and remove it or something
                arrow.SetActive(false);
                continue;
            }
            
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
