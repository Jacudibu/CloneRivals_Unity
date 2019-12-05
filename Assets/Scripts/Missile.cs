﻿using System;
using Extensions;
using UnityEngine;

[Serializable]
public struct MissileData
{
    public float validAngle;
    public float reloadTime;
    
    public int damage;
    public int speed;
    public int rotationDegrees;
}

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    private MissileData _data;
    
    private Transform _target;
    private int _ownerId;
    
    void Update()
    {
        if (_target != null)
        {
            var targetRotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _data.rotationDegrees);
        }
        
        transform.Translate(Time.deltaTime * _data.speed * Vector3.forward);
    }
    
    public void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>() ?? other.FindInAllParents<PlayerController>();
        if (playerController != null && _ownerId == playerController.gameObject.GetInstanceID())
        {
            return;
        }
        
        var targetable = other.GetComponent<TargetableObject>();
        if (targetable != null)
        {
            targetable.TakeDamage(_data.damage);
        }
        
        Instantiate(explosionEffect, transform.position, transform.rotation);
        Destroy(gameObject, 1f);

        foreach (var meshRenderer in GetComponentsInChildren<MeshRenderer>())
        {
            meshRenderer.enabled = false;
        }
        foreach (var particles in GetComponentsInChildren<ParticleSystem>())
        {
            particles.Stop();
        }
        
        transform.GetChild(0).gameObject.SetActive(false);
        enabled = false;
    }

    public void SetTarget(Transform newTarget)
    {
        _target = newTarget;
    }

    public void SetData(MissileData data)
    {
        _data = data;
    }

    public void SetOwnerId(int id)
    {
        _ownerId = id;
    }
}
