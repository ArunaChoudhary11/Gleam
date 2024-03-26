using System.Collections;
using UnityEngine;

public class EnemyBehavior : MonoBehaviour
{
    public bool isFlashed;
    public bool isRefracted;
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
    public void Attack()
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
        attackGO.GetComponent<Laser>().SetValues(attackSpeed, attackRaySize, damage);
    }
    public void DeviationEffectReset()
    {
        isRefracted = false;
    }
}