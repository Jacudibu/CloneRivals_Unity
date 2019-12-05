using UnityEngine;

public class TargetableObject : MonoBehaviour
{
    [SerializeField] private int currentHealth;
    [SerializeField] private int maxHealth;

    private void Start()
    {
        currentHealth = maxHealth;
    }
    
    public int GetHealth()
    {
        return currentHealth;
    }

    public int RestoreHealth(int amount)
    {
        currentHealth += amount;
        if (currentHealth > maxHealth)
        {
            currentHealth = maxHealth;
        }

        return currentHealth;
    }

    public int TakeDamage(int amount)
    {
        currentHealth -= amount;

        if (currentHealth <= 0)
        {
            Die();
        }
        
        return currentHealth;
    }

    private void Die()
    {
        EventHub.OnTargetableDestroyed.Invoke(gameObject);

        // TODO: Explosion particles & Sound
        
        var rb = GetComponent<Rigidbody>();
        var pc = GetComponent<PlayerController>();
        if (rb != null && pc != null)
        {
            rb.velocity = pc.currentSpeed * transform.forward;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.drag = 0;
            rb.angularDrag = 0.05f;
            
            // TODO: Fire particles
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
