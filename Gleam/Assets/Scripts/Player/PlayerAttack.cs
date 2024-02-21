using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    [SerializeField] private GameObject AttackPoint;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AttackPower;
    private TestHealth enemyHealth;
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left Mouse btn input
        {
            GameObject newAttackPoint = Instantiate(AttackPoint, transform.position, Quaternion.identity);
            Vector2 endPoint = PlayerManager.Instance.GetClosestPoint();

            Collider2D enemy = PlayerManager.Instance.ClosestEnemy;

            if(enemy != null)
            {
                enemyHealth = enemy.GetComponent<TestHealth>();

                if(PlayerManager.Instance.canUse_Refract == true) PlayerManager.Instance.refract.DeviationEffect(enemy.GetComponent<Enemytest>());
            }
            else enemyHealth = null;
            
            PrimaryAttack attack = newAttackPoint.GetComponent<PrimaryAttack>();
            attack.SetValues(endPoint, enemyHealth, AttackPower, AttackSpeed);
        }
    }
}