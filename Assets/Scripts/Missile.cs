using System;
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
    public float maximumLifetime;
}

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    private MissileData _data;
    
    private Transform _target;
    private int _ownerId;
    private float _totalLifetime;
    
    void Update()
    {
        if (_target != null)
        {
            var targetRotation = Quaternion.LookRotation(_target.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * _data.rotationDegrees);
        }
        
        transform.Translate(Time.deltaTime * _data.speed * Vector3.forward);
        _totalLifetime += Time.deltaTime;

        if (_totalLifetime > _data.maximumLifetime)
        {
            DestroyMissile();
        }
    }
    
    public void OnTriggerEnter(Collider other)
    {
        var playerController = other.GetComponent<PlayerController>() ?? other.FindInAllParents<PlayerController>();
        if (playerController != null && (_ownerId == playerController.gameObject.GetInstanceID() || playerController.isRolling))
        {
            return;
        }
        
        TargetableObject targetable;
        if (playerController == null)
        {
            targetable = other.GetComponent<TargetableObject>() ?? other.FindInAllParents<TargetableObject>();
        }
        else
        {
            targetable = playerController.GetComponent<TargetableObject>();
        }
        
        if (targetable != null)
        {
            targetable.TakeDamage(_data.damage);
        }
        
        DestroyMissile();
    }

    private void DestroyMissile()
    {
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
