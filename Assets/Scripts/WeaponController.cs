using InputConfiguration;
using UnityEngine;

[RequireComponent(typeof(TargetManager))]
public class WeaponController : MonoBehaviour
{
    public float secondaryReattackTime = 1f;
    public bool SecondaryLockable { get; private set; }
    
    private float _lastSecondaryAttackTime;
    
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] missileSpawnPoints;
    
    [SerializeField] public float lockOnAngle = 12;
    [SerializeField] public float lockOnRange = 165;
    
    private Transform _shipTransform;
    private TargetManager _targetManager;

    [SerializeField] private int missileDamage = 10;
    [SerializeField] private int missileSpeed = 55;
    
    void Start()
    {
        _shipTransform = GetComponentInChildren<MeshRenderer>().transform;
        _targetManager = GetComponent<TargetManager>();
    }
    
    void Update()
    {
        if (_targetManager.Target != null)
        {
            var angle = Vector3.Angle(_shipTransform.forward, _targetManager.Target.transform.position - _shipTransform.position);
            var distance = Vector3.Distance(_shipTransform.position, _targetManager.Target.transform.position);
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
                    missile.SetTarget(SecondaryLockable ? _targetManager.Target.transform : null);
                    missile.SetDamage(missileDamage);
                    missile.SetSpeed(missileSpeed);
                    missile.SetOwnerId(gameObject.GetInstanceID());
                }

                _lastSecondaryAttackTime = Time.time;
            }
        }
    }
}
