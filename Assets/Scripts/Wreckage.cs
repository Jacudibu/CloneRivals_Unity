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
        Destroy(_rb);
        _collider.enabled = false;
        
        // TODO: Spawn explosion particles
        transform.position = collision.GetContact(0).point;
        enabled = false;
    }
}
