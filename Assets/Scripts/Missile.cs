using System;
using UnityEngine;

public class Missile : MonoBehaviour
{
    [SerializeField] private GameObject explosionEffect;
    
    [SerializeField] private Transform target;

    [SerializeField] private float speed = 50;
    [SerializeField] private float rotationDegrees = 110;
    
    void Update()
    {
        if (target != null)
        {
            var targetRotation = Quaternion.LookRotation(target.position - transform.position);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, Time.deltaTime * rotationDegrees);
        }
        
        transform.Translate(Time.deltaTime * speed * Vector3.forward);
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
        target = newTarget;
    }
}
