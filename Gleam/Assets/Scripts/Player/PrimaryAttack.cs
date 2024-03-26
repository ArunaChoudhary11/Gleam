using UnityEngine;

public class PrimaryAttack : MonoBehaviour
{
    private Vector2 _endPoint;
    private HealthSystem _enemyHealth;
    private float _attackPower;
    private float _attackSpeed;
    private bool attacking;
    public Color flashColor = Color.white;
    [SerializeField] private LayerMask enemyMask;
    void Start()
    {
        Destroy(gameObject, 2.5f);
    }
    public void SetValues(Vector2 endPoint, HealthSystem enemyHealth, float power, float attackSpeed)
    {
        _endPoint = endPoint;
        _enemyHealth = enemyHealth;
        _attackPower = power;
        _attackSpeed = attackSpeed;
        attacking = true;
    }

    void Update()
    {
        if(attacking == true)
        {
            transform.position = Vector2.Lerp(transform.position, _endPoint, _attackSpeed * Time.deltaTime);
            
            float AttackDistance = Vector3.Distance(transform.position, _endPoint);

            if (AttackDistance <= 0.1f)
            {
                if(_enemyHealth != null)
                {
                    _enemyHealth.TakeDamage(_attackPower);
                    ImpactEffects.Instance.FlashOnImpact(_enemyHealth.GetComponent<SpriteRenderer>(), 0.2f, flashColor);
                }
                else
                {
                    Collider2D col = Physics2D.OverlapCircle(transform.position, 1.5f, enemyMask);

                    if(col != null)
                    {
                        HealthSystem enemyHealth = col.GetComponent<HealthSystem>();
                        enemyHealth.TakeDamage(_attackPower);
                        ImpactEffects.Instance.FlashOnImpact(enemyHealth.GetComponent<SpriteRenderer>(), 0.2f, flashColor);
                    }
                }
                
                Destroy(gameObject);
            }
        }
    }
}