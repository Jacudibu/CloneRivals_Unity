using Extensions;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;

    [SerializeField] private float rotationDegrees = 110;

    private Transform _target;
    private int _damage;
    private int _speed;
    private int _ownerId;

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
        var playerController = other.GetComponent<PlayerController>() ?? other.FindInAllParents<PlayerController>();
        if (playerController != null && _ownerId == playerController.gameObject.GetInstanceID())
        {
            return;
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

    public void SetOwnerId(int id)
    {
        _ownerId = id;
    }
}
