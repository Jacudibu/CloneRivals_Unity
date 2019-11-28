using System.Collections;
using System.Collections.Generic;
using InputConfiguration;
using UnityEngine;

public class WeaponController : MonoBehaviour
{
    public float SecondaryReattackTime = 1f;
    private float _lastSecondaryAttackTime;
    
    
    void Start()
    {
        
    }

    void Update()
    {
        if (KeyBindings.IsSecondary())
        {
            if (Time.time - _lastSecondaryAttackTime < SecondaryReattackTime)
            {
                // TODO: Pew pew
            }
        }
    }
}
