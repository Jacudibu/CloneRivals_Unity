using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{
    [SerializeField] private float lifeTime = 1;
    
    void Start()
    {
        Destroy(gameObject, lifeTime);
    }
}
