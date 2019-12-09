using InputConfiguration;
using UnityEngine;

[RequireComponent(typeof(TargetManager))]
[DisallowMultipleComponent]
public class WeaponController : MonoBehaviour
{
    public bool IsMissileLockable { get; private set; }
    
    private float _lastMissileAttackTime;
    
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] missileSpawnPoints;
    
    [SerializeField] public float lockOnRange = 165;
    
    private Transform _shipTransform;
    private TargetManager _targetManager;
    private PlayerController _playerController;

    [SerializeField] private MissileData missileData;
    
    void Start()
    {
        _shipTransform = GetComponentInChildren<MeshRenderer>().transform;
        _targetManager = GetComponent<TargetManager>();
        _playerController = GetComponent<PlayerController>();
    }
    
    void Update()
    {
        if (_targetManager.Target != null)
        {
            var angle = Vector3.Angle(_shipTransform.forward, _targetManager.Target.transform.position - _shipTransform.position);
            var distance = Vector3.Distance(_shipTransform.position, _targetManager.Target.transform.position);
            IsMissileLockable = angle > -missileData.validAngle &&
                                angle < missileData.validAngle && 
                                distance <= lockOnRange;
        }
        else
        {
            IsMissileLockable = false;
        }

        if (_playerController != null && _playerController.isRolling)
        {
            return;
        }
        
        
        if (KeyBindings.IsPrimary())
        {
            
        }

        if (KeyBindings.IsFireMissile())
        {
            if (Time.time - _lastMissileAttackTime > missileData.reloadTime)
            {
                foreach (var missileSpawnPoint in missileSpawnPoints)
                {
                    var obj = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
                    var missile = obj.GetComponent<Missile>();
                    missile.SetTarget(IsMissileLockable ? _targetManager.Target.transform : null);
                    missile.SetData(missileData);
                    missile.SetOwnerId(gameObject.GetInstanceID());
                }

                _lastMissileAttackTime = Time.time;
            }
        }
    }
}
