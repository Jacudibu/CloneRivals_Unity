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
        Destroy(gameObject, 0.1f);
    }
}
