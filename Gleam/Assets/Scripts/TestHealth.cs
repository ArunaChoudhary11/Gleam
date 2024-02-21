using UnityEngine;

public class TestHealth : MonoBehaviour
{
    public float health;
    public void TakeDamage(float _damage)
    {
        health -= _damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
}