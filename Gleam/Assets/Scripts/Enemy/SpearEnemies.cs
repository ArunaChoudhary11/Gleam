using System.Collections.Generic;
using UnityEngine;

public class SpearEnemies : MonoBehaviour
{
    //  Thigs Left:
    //      Character Sprite
    //      Character Animations
    //      Paticle Effects

    public enum Phase {Idle, Chase, Attack, Retreat}
    [SerializeField] private float health;
    [SerializeField] private Phase enemyPhase;
    private List<Vector2> points = new List<Vector2>();
    [SerializeField] private float range = 3f;
    private float pointsTimer;
    [SerializeField] private float attackActivationTimer;
    [SerializeField] private float attackSpeed;
    [SerializeField] private float retreatSpeed;
    [SerializeField] private float speed;
    private int currentPointIndex;
    private Vector2 retreatPoint;
    private Vector2 attackPoint;
    [SerializeField] private float enemyRange;
    [SerializeField] private Transform player;
    [SerializeField] private float damage;
    [SerializeField] private Animator animator;
    [SerializeField] private LayerMask playerMask;
    private bool attacked;
    private bool isDefended;
    
    void Start()
    {
        enemyPhase = Phase.Idle;
        Move();
    }
    void Update()
    {
        switch(enemyPhase)
        {
            case Phase.Idle: Idle(); break;

            case Phase.Chase: Chase(); break;

            case Phase.Attack: Attack(); break;

            case Phase.Retreat: Retreat(); break;
        }
    }
    private void Retreat()
    {
        isDefended = false;
        transform.position = Vector2.MoveTowards(transform.position, retreatPoint, retreatSpeed * Time.deltaTime);

        float distance = Vector2.Distance(transform.position, retreatPoint);

        if(distance <= 1)
        {
            Move();
            enemyPhase = Phase.Idle;
        }
    }
    private void Chase()
    {
        isDefended = true;
        pointsTimer = 0;
        float distance = Vector2.Distance(transform.position, player.position);

        if(distance > range)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.position, retreatSpeed * Time.deltaTime);
            return;
        }

        Move();
        enemyPhase = Phase.Idle;
    }
    private void Idle()
    {
        isDefended = true;
        float playerDistance = Vector2.Distance(transform.position, player.position);

        if(playerDistance > range * 2) enemyPhase = Phase.Chase; 

        if(attacked == true) return;

        if(pointsTimer >= attackActivationTimer)
        {
            attackActivationTimer = Random.Range(3f, 6f);
            attackPoint = player.position;
            attacked = true;

            Invoke(nameof(InitalizeAttack), 1);
        }

        pointsTimer += Time.deltaTime;

        if(points.Count > 0)
        {
            float distance = Vector2.Distance(transform.position, points[currentPointIndex]);

            if(distance <= 0.2f)
            {
                currentPointIndex++;
                if(currentPointIndex >= points.Count) currentPointIndex = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[currentPointIndex], speed * Time.deltaTime);
    }
    private void Move()
    {
        pointsTimer = 0;
        points.Clear();

        for(int i = 0; i < 4; i++)
        {
            Vector2 point = PointsArea(transform.position, Random.Range(0, enemyRange));
            Debug.DrawRay(point, Vector2.up, Color.white, 5);
            points.Add(point);
        }

        currentPointIndex = Random.Range(0, points.Count);
    }
    private Vector2 PointsArea(Vector3 center, float radius){
		
        float ang = Random.Range(0, 180);
		Vector2 pos;
		pos.x = center.x + radius * Mathf.Sin(ang * Mathf.Deg2Rad);
		pos.y = center.y + radius * Mathf.Cos(ang * Mathf.Deg2Rad);
		return pos;
	}
    private void Attack()
    {
        isDefended = false;
        if(attacked == false)
        {
            Vector2 direction = (attackPoint - (Vector2) transform.position).normalized;
            float distance = Vector2.Distance(attackPoint, transform.position);
            transform.up = direction;
        
            if(distance > 1)
            {
                transform.position = Vector2.MoveTowards(transform.position, attackPoint, attackSpeed * Time.deltaTime);
                return;
            }

            retreatPoint = new Vector2(Random.Range(transform.position.x - (range / 2), transform.position.x - (range / 2)), Random.Range(transform.position.y, transform.position.y + range));

            DamagePlayer();
            transform.up = Vector2.up;
            Invoke(nameof(InitalizeRetreat), 1);
            attacked = true;
        }
    }
    private void InitalizeRetreat()
    {
        attacked = false;
        enemyPhase = Phase.Retreat;
    }
    private void InitalizeAttack()
    {
        attacked = false;
        enemyPhase = Phase.Attack;
    }
    private void DamagePlayer()
    {
        Collider2D col = Physics2D.OverlapCircle(transform.position, 3, playerMask);
        
        if(col != null)
        {
            float distance = Vector2.Distance(transform.position, col.transform.position);
            
            float playerDamage = damage / distance;

            Debug.Log(playerDamage);

            playerDamage = Mathf.Clamp(playerDamage, 0, damage);

            PlayerManager.Instance.TakeDamage(playerDamage);
        }
    }
    public void TakeDamage(float damage)
    {
        if(isDefended == false) health -= damage;
        else health -= damage / 10;

        if(health <= 0)
        {
            health = 0;
            Dead();
        }
    }
    private void Dead()
    {
        Destroy(gameObject);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.white;
        Gizmos.DrawWireSphere(transform.position, enemyRange);
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}