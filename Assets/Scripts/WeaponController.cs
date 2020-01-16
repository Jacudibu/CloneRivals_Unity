using Settings.InputConfiguration;
using UnityEngine;
using UnityEngine.Serialization;

[RequireComponent(typeof(TargetManager))]
[DisallowMultipleComponent]
public class WeaponController : MonoBehaviour
{
    public bool IsMissileLockable { get; private set; }

    private float _lastGunAttackTime;
    private float _lastMissileAttackTime;

    [SerializeField] private GameObject gunPrefab;
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] gunSpawnPoints;
    [SerializeField] private Transform[] missileSpawnPoints;
    
    [SerializeField] public float lockOnRange = 165;
    
    private Transform _shipTransform;
    private TargetManager _targetManager;
    private PlayerController _playerController;

    [SerializeField] private ProjectileData gunData;
    [SerializeField] private ProjectileData missileData;

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

        if (_playerController == null || _playerController.isRolling)
        {
            return;
        }
        
        
        if (KeyBindings.PrimaryWeapon.IsPressed())
        {
            FireGun();
        }

        if (KeyBindings.FireMissile.IsPressed())
        {
            FireMissile();
        }
    }

    public void FireGun()
    {
        if (Time.time - _lastGunAttackTime > gunData.reloadTime)
        {
            foreach (var gunSpawnPoint in gunSpawnPoints)
            {
                var obj = Instantiate(gunPrefab, gunSpawnPoint.position, gunSpawnPoint.rotation);
                var projectile = obj.GetComponent<Projectile>();
                projectile.SetData(gunData);
                projectile.SetOwnerId(gameObject.GetInstanceID());
            }

            _lastGunAttackTime = Time.time;
        }
    }

    public void FireMissile()
    {
        if (Time.time - _lastMissileAttackTime > missileData.reloadTime)
        {
            foreach (var missileSpawnPoint in missileSpawnPoints)
            {
                var obj = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
                var missile = obj.GetComponent<Projectile>();
                missile.SetTarget(IsMissileLockable ? _targetManager.Target.transform : null);
                missile.SetData(missileData);
                missile.SetOwnerId(gameObject.GetInstanceID());
            }

            _lastMissileAttackTime = Time.time;
        }
    }
}
