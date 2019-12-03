using System.Collections;
using System.Collections.Generic;
using InputConfiguration;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float SecondaryReattackTime = 1f;
    public bool SecondaryLockable { get; private set; }
    
    private float _lastSecondaryAttackTime;
    
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] missileSpawnPoints;
    [SerializeField] private Transform target;
    [SerializeField] public float lockOnAngle = 12;

    private Transform _shipTransform;

    
    void Start()
    {
        _shipTransform = GetComponentInChildren<MeshRenderer>().transform;
    }
    
    void Update()
    {
        var angle = Vector3.Angle(_shipTransform.forward, target.transform.position - _shipTransform.position);
        SecondaryLockable = angle > -lockOnAngle && angle < lockOnAngle;
        
        if (KeyBindings.IsSecondary())
        {
            if (Time.time - _lastSecondaryAttackTime > SecondaryReattackTime)
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
