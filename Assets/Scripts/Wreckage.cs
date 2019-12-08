using Effects;
using UnityEngine;

public class Wreckage : MonoBehaviour
{
    private Rigidbody _rb;
    private Collider _collider;
    
    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        _collider = GetComponent<Collider>();
        var colliders = GetComponentsInChildren<Collider>();
        foreach (var col in colliders)
        {
            if (col == _collider)
            {
                continue;
            }
            
            col.enabled = false;
        }
    }
    
    private void Update()
    {
        transform.rotation = Quaternion.LookRotation(_rb.velocity);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (!enabled)
        {
            return;
        }
        
        Destroy(_rb);
        _collider.enabled = false;
        
        var collisionPoint =  collision.GetContact(0).point;
        transform.position = collisionPoint;

        Instantiate(EffectCollection.GetShipCrashEffect(), collisionPoint + Vector3.up, Quaternion.identity, transform);
        Instantiate(EffectCollection.GetSmallFire(), collisionPoint + Vector3.up, Quaternion.identity, transform);
        enabled = false;
    }
}
