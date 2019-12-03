using InputConfiguration;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float secondaryReattackTime = 1f;
    public bool SecondaryLockable { get; private set; }
    
    private float _lastSecondaryAttackTime;
    
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] missileSpawnPoints;
    [SerializeField] private Transform target;
    
    [SerializeField] public float lockOnAngle = 12;
    [SerializeField] public float lockOnRange = 165;
    
    private Transform _shipTransform;

    
    void Start()
    {
        _shipTransform = GetComponentInChildren<MeshRenderer>().transform;
    }
    
    void Update()
    {
        if (target != null)
        {
            var angle = Vector3.Angle(_shipTransform.forward, target.transform.position - _shipTransform.position);
            var distance = Vector3.Distance(_shipTransform.position, target.position);
            SecondaryLockable = angle > -lockOnAngle &&
                                angle < lockOnAngle && 
                                distance <= lockOnRange;
        }
        else
        {
            SecondaryLockable = false;
        }

        if (KeyBindings.IsPrimary())
        {
            
        }

        if (KeyBindings.IsSecondary())
        {
            if (Time.time - _lastSecondaryAttackTime > secondaryReattackTime)
            {
                foreach (var missileSpawnPoint in missileSpawnPoints)
                {
                    var obj = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
                    var missile = obj.GetComponent<Missile>();
                    missile.SetTarget(SecondaryLockable ? target : null);
                }

                _lastSecondaryAttackTime = Time.time;
            }
        }
    }
}
