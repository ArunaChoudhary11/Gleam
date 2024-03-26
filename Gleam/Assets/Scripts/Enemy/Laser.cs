using UnityEngine;

public class Laser : MonoBehaviour
{
    private float _attackSpeed;
    private float _attackRaySize;
    private float _damage;
    private bool attacking;
    private Vector2 startPoint;
    [SerializeField] private LayerMask mask;
    void Start()
    {
        startPoint = transform.position;
    }
    public void SetValues(float attackSpeed, float attackRaySize, float damage)
    {
        Destroy(gameObject, 2f);
        _attackSpeed = attackSpeed;
        _attackRaySize = attackRaySize;
        _damage = damage;
        attacking = true;
    }
    void FixedUpdate()
    {
        if(attacking == true)
        {
            transform.position += transform.up * _attackSpeed * Time.deltaTime;
            
            if(Vector2.Distance((Vector2) transform.position, startPoint) > _attackRaySize)
            {
                startPoint += (Vector2) transform.up * _attackSpeed * Time.deltaTime;
            }
        }

        AttackRay();
    }
    private void AttackRay()
    {
        Vector2 direction = (Vector2) transform.position - startPoint;
        direction = direction.normalized;
        float distance = Vector2.Distance(startPoint, transform.position);

        RaycastHit2D hit = Physics2D.Raycast(startPoint, direction, distance, mask);
        Debug.DrawRay(startPoint, direction * distance, Color.magenta);

        if(hit.collider != null)
        {
            HittingPhotonShield(hit);

            if(hit.collider.TryGetComponent<HealthSystem>(out var health))
            {
                float hitPointDistance = Vector2.Distance(hit.point, (Vector2) transform.position);
                hitPointDistance = Mathf.Clamp(hitPointDistance, 1f, 3f) * 5;
                float damageAmount = _damage / hitPointDistance;
                Debug.Log("sfdgbf " + damageAmount);
                health.TakeDamage(damageAmount);
            }
            
            Destroy(gameObject);
        }
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.black;
        Gizmos.DrawWireSphere(startPoint, 0.1f);
        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
    private void HittingPhotonShield(RaycastHit2D hit)
    {
        if(hit.collider.TryGetComponent<PhotonShield>(out var shield))
        {
            if(shield.isUsed == true)
            {
                shield.Shield(hit.point, hit.normal);
                shield.isUsed = false;
            }
        }
    }
}