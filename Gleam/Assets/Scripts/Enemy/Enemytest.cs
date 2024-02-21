using System.Collections;
using UnityEngine;

public class Enemytest : MonoBehaviour
{
    public bool isFlashed;
    public bool isRefracted;
    public float speed;
    public GameObject attackPrefab;

    [Header("Attack")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRaySize;
    [SerializeField] private float damage;
    [SerializeField] private float attackCountdown;
    private float attackDelay;
    private float attackDirectionError = 0;
    void Start()
    {
        Attack();
    }

    public IEnumerator Flashbang(float duration)
    {
        isFlashed = true;
        ImpactEffects.Instance.FlashOnImpact(GetComponent<SpriteRenderer>(), duration, Color.white);
        yield return new WaitForSeconds(duration);
        isFlashed = false;
    }
    void Update()
    {
        Move();

        if(isFlashed)
        {
            Debug.Log(name + " enemyFlashed");
        }

        if(attackDelay >= attackCountdown)
        {
            attackDelay = 0f;
            Attack();
        }

        attackDelay += Time.deltaTime;
    }
    private void Attack()
    {
        if(isRefracted == true)
        {
            attackDirectionError = Random.Range(5f,60f);
            int changeDir = Random.Range(0,2);
            attackDirectionError = changeDir == 0 ? attackDirectionError: -attackDirectionError;
        }
        else
        {
            attackDirectionError = 0;
        }

        GameObject attackGO = Instantiate(attackPrefab, transform.position, Quaternion.identity);
        Vector2 attackDirection = Quaternion.AngleAxis(attackDirectionError / 2, Vector3.forward) * transform.right;
        attackGO.transform.up = attackDirection;
        attackGO.GetComponent<EnemyTestAttack>().SetValues(attackSpeed, attackRaySize, damage);
    }
    float timer;
    int direction = 1;
    private void Move()
    {
        transform.Translate(transform.right * direction * speed * SpeedTimeDilation.Instance.globalSpeedDeltaTime * Time.deltaTime);

        if(timer >= 2)
        {
            timer = 0;
            direction *= -1;
        }
        
        timer += Time.deltaTime * SpeedTimeDilation.Instance.globalSpeedDeltaTime;
    }
    public void DeviationEffectReset()
    {
        isRefracted = false;
    }
}