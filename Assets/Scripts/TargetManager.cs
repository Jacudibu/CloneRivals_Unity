using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetManager : MonoBehaviour
{
    public List<GameObject> thingsTargetingMe;
    public GameObject target;

    public GameObject targetArrow;
    public GameObject targetMeArrow;
    private List<GameObject> _thingsTargetingMeArrows;
    
    public float radius = 100;
    public float arrowRotationOffset = -90;

    private void Start()
    {
        _thingsTargetingMeArrows = new List<GameObject>()
        {
            targetMeArrow
        };
    }

    private void Update()
    {
        if (target != null)
        {
            AdjustArrowTransform(targetArrow.transform, target.transform.position);
        }

        if (_thingsTargetingMeArrows.Count < thingsTargetingMe.Count)
        {
            var original = _thingsTargetingMeArrows[0];

            while (_thingsTargetingMeArrows.Count < thingsTargetingMe.Count)
            {
                var arrow = Instantiate(original, original.transform.parent, true);
                _thingsTargetingMeArrows.Add(arrow);
            }
        }

        for (var i = 0; i < thingsTargetingMe.Count; i++)
        {
            AdjustArrowTransform(_thingsTargetingMeArrows[i].transform, thingsTargetingMe[i].transform.position);
        }
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
