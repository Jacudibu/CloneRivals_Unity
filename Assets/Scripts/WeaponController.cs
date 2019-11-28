using System.Collections;
using System.Collections.Generic;
using InputConfiguration;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float SecondaryReattackTime = 1f;
    private float _lastSecondaryAttackTime;
    
    [SerializeField] private GameObject missilePrefab;
    [SerializeField] private Transform[] missileSpawnPoints;
    [SerializeField] private Transform target;
    
    void Update()
    {
        if (KeyBindings.IsSecondary())
        {
            if (Time.time - _lastSecondaryAttackTime > SecondaryReattackTime)
            {
                foreach (var missileSpawnPoint in missileSpawnPoints)
                {
                    var obj = Instantiate(missilePrefab, missileSpawnPoint.position, missileSpawnPoint.rotation);
                    var missile = obj.GetComponent<Missile>();
                    missile.SetTarget(target);
                }

                _lastSecondaryAttackTime = Time.time;
            }
        }
    }
}
