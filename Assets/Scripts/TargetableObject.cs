using Effects;
using UnityEngine;

[DisallowMultipleComponent]
public class TargetableObject : MonoBehaviour
{
    [SerializeField] private int maxStructure;
    [SerializeField] private int maxShield;

    public float Structure { get; private set; }
    public float Shield { get; private set; }
    public bool IsTargetable { get; private set; } = true;

    public readonly EventHub.TargetableObjectEvent OnHealthChanged = new EventHub.TargetableObjectEvent();

    public float GetStructurePercentage() => Structure / maxStructure;
    public float GetShieldPercentage() => Shield / maxShield;
    
    void Start()
    {
        Structure = maxStructure;
        Shield = maxShield;
    }
    
    public void RestoreStructure(int amount)
    {
        Structure += amount;
        if (Structure > maxStructure)
        {
            Structure = maxStructure;
        }

        OnHealthChanged.Invoke(this);
    }
    
    public void RestoreShield(int amount)
    {
        Shield += amount;
        if (Shield > maxShield)
        {
            Shield = maxShield;
        }

        OnHealthChanged.Invoke(this);
    }

    public void TakeDamage(int amount)
    {
        Shield -= amount;

        if (Shield < 0)
        {
            var remaining = -Shield;
            Shield = 0;

            Structure -= remaining;
        }
        
        if (Structure <= 0)
        {
            Die();
            Structure = 0;
        }

        OnHealthChanged.Invoke(this);
    }

    private void Die()
    {
        if (!IsTargetable)
        {
            return;
        }
        
        IsTargetable = false;
        
        EventHub.OnTargetableDestroyed.Invoke(this);

        // TODO: Explosion particles & Sound
        Instantiate(EffectCollection.GetShipKillEffect(), transform.position, new Quaternion());
        
        var rb = GetComponent<Rigidbody>();
        var engine = GetComponent<Engine>();
        if (rb != null && engine != null)
        {
            rb.velocity = engine.currentSpeed * transform.forward;
            rb.useGravity = true;
            rb.isKinematic = false;
            rb.drag = 0;
            rb.angularDrag = 0.05f;
            
            engine.enabled = false;

            var engineParticles = engine.GetComponentsInChildren<ParticleSystem>();
            foreach (var particles in engineParticles)
            {
                particles.Stop();
            }

            var playerController = engine.GetComponent<PlayerController>();
            if (playerController != null)
            {
                playerController.enabled = false;
            }

            // TODO: Fire particles

            engine.gameObject.AddComponent<Wreckage>();
            enabled = false;
        }
        else
        {
            Destroy(gameObject, 0.1f);
        }
    }
}
