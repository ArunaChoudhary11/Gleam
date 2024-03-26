using UnityEngine;

public class HealthSystem : MonoBehaviour
{
    [SerializeField] private float health;
    public float Health() { return health; }
    public void TakeDamage(float _damage)
    {
        health -= _damage;
        
        if (health <= 0) health = 0;
    }
}