using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject AttackPoint;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AttackPower;
    private HealthSystem enemyHealth;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left Mouse btn input
        {
            GameObject newAttackPoint = Instantiate(AttackPoint, transform.position, Quaternion.identity);
            Vector2 endPoint = PlayerManager.Instance.GetClosestPoint();

            Collider2D enemy = PlayerManager.Instance.ClosestEnemy;

            if(enemy != null)
            {
                enemyHealth = enemy.GetComponent<HealthSystem>();

                if(PlayerManager.Instance.canUse_Refract == true) PlayerManager.Instance.refract.DeviationEffect(enemy.GetComponent<EnemyBehavior>());
            }
            else enemyHealth = null;
            
            PrimaryAttack attack = newAttackPoint.GetComponent<PrimaryAttack>();
            attack.SetValues(endPoint, enemyHealth, AttackPower, AttackSpeed);
        }
    }
}