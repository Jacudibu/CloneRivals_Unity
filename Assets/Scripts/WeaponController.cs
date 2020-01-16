using System.Collections;
using Settings.InputConfiguration;
using UnityEngine;

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
        if (Time.time - _lastGunAttackTime < gunData.reloadTime)
        {
            return;
        }
        
        if (gunData.salvos == 1)
        {
            FireProjectile(gunData, gunPrefab, gunSpawnPoints, null);
        }
        else
        {
            StartCoroutine(FireProjectileCoroutine(gunData, gunPrefab, gunSpawnPoints, null));
        }

        _lastGunAttackTime = Time.time;
    }

    public void FireMissile()
    {
        if (Time.time - _lastMissileAttackTime < missileData.reloadTime)
        {
            return;
        }
        
        var target = IsMissileLockable ? _targetManager.Target.transform : null;

        if (missileData.salvos == 1)
        {
            FireProjectile(missileData, missilePrefab, missileSpawnPoints, target);
        }
        else
        {
            StartCoroutine(FireProjectileCoroutine(missileData, missilePrefab, missileSpawnPoints, target));
        }

        _lastMissileAttackTime = Time.time;
    }

    private IEnumerator FireProjectileCoroutine(ProjectileData data, GameObject prefab, Transform[] spawnPoints, Transform target)
    {
        for (var i = 0; i < data.salvos; i++)
        {
            FireProjectile(data, prefab, spawnPoints, target);
            yield return new WaitForSeconds(data.timeBetweenSalvos);
        }
    }

    private void FireProjectile(ProjectileData data, GameObject prefab, Transform[] spawnPoints, Transform target)
    {
        foreach (var spawnPoint in spawnPoints)
        {
            var obj = Instantiate(prefab, spawnPoint.position, spawnPoint.rotation);
            var projectile = obj.GetComponent<Projectile>();
            projectile.SetTarget(target);
            projectile.SetData(data);
            projectile.SetOwnerId(gameObject.GetInstanceID());
        }
    }
}
