﻿using System;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;

    [SerializeField] private float rotationDegrees = 110;

    private Transform _target;
    private int _damage;
    private int _speed;

    void Update()
    {
        if (_target != null)
        {
            var targetRotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationDegrees);
        }
        
        transform.Translate(Time.deltaTime * _speed * Vector3.forward);
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.transform.parent != null)
        {
            if (other.transform.parent.parent != null)
            {
                if (other.transform.parent.parent.parent != null)
                {
                    if (other.transform.parent.parent.parent.GetComponent<PlayerController>())
                    {
                        return;
                    }
                }
            }
        }

        var targetable = other.GetComponent<TargetableObject>();
        if (targetable != null)
        {
            targetable.TakeDamage(_damage);
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

    public void SetDamage(int amount)
    {
        _damage = amount;
    }

    public void SetSpeed(int amount)
    {
        _speed = amount;
    }
}
