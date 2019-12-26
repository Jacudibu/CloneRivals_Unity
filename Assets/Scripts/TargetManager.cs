using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[DisallowMultipleComponent]
public class TargetManager : MonoBehaviour
{
    public List<GameObject> thingsTargetingMe;
    public GameObject Target { get; private set; }

    private Plane[] _cameraFrustumPlanes;

    private Transform _shipTransform;
    [SerializeField] private float targetLockRange = 200f;
    [SerializeField] private float targetLockAngle = 25f;

    public readonly EventHub.TargetableObjectEvent OnTargetUnLock = new EventHub.TargetableObjectEvent();
    public readonly EventHub.TargetableObjectEvent OnTargetLock = new EventHub.TargetableObjectEvent();
    
    public delegate void OnTargetingMeDelegate();
    public event OnTargetingMeDelegate OnIncomingTargetUpdate;
    
    private void Start()
    {
        _shipTransform = GetComponentInChildren<MeshRenderer>().transform;

        EventHub.OnTargetableDestroyed.AddListener(OnTargetableDestroyed);
    }

    public void SearchForTarget()
    {
        var viableTargets = Physics.OverlapSphere(_shipTransform.position, targetLockRange)
            .Select(x => x.GetComponent<TargetableObject>())
            .Where(x => x != null)
            .Where(x => x.IsTargetable)
            .Where(x => Mathf.Abs(Vector3.Angle(_shipTransform.forward, x.transform.position - _shipTransform.position)) < targetLockAngle)
            .Where(x => x.gameObject != gameObject)
            .Where(x => x.gameObject != Target)
            .Select(x => x.gameObject);

        if (Target != null)
        {
            OnTargetUnLock.Invoke(Target.GetComponent<TargetableObject>());
        }

        Target = viableTargets.FirstOrDefault();
        
        if (Target != null)
        {
            OnTargetLock.Invoke(Target.GetComponent<TargetableObject>());
        }
    }

    private void OnTargetableDestroyed(TargetableObject targetable)
    {
        if (targetable.gameObject == Target)
        {
            OnTargetUnLock.Invoke(targetable);
            Target = null;
        }
        
        thingsTargetingMe.Remove(targetable.gameObject);
        OnIncomingTargetUpdate?.Invoke();
    }
}
