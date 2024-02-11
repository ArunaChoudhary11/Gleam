using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerAttack : MonoBehaviour
{
    public float Focus;
    public float intensity;
    public float Wavelength;
    public bool hasSecondWeapon;
    public SecondaryWeapon secondaryWeapon;

    public GameObject AttackPoint;
    public float AttackSpeed;
    public LayerMask enemeyMask;
    public bool isAttacking;
    public Vector2 endPoint;
    public float radius;
    Collider2D ClostestEnemy;
    public Testhealth enemyHealth;
    public float AttackPower;


    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left Mouse btn input
        {
            enemyHealth = null;
            AttackPoint.GetComponent<TrailRenderer>().enabled = false;
            AttackPoint.transform.position = transform.position;
            AttackPoint.GetComponent<TrailRenderer>().enabled = true;

            isAttacking = true;

            endPoint = GetEnemyPosition();
        }

        if (isAttacking == true)
        {
            Attack(endPoint);
        }

        float AttackDistance = Vector3.Distance(AttackPoint.transform.position, endPoint);
        if (AttackDistance <= 0.1f)
        {
            if (isAttacking == true)
            {

                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(AttackPower);
                }
            }
            isAttacking = false;
            AttackPoint.GetComponent<TrailRenderer>().enabled = false;
        }
    }

    private void Attack(Vector2 endPoint)
    {
        Debug.Log("Attack");
        AttackPoint.transform.position = Vector2.Lerp(AttackPoint.transform.position, endPoint, AttackSpeed * Time.deltaTime);
    }

    private void SecondWeapon()
    {
        if (hasSecondWeapon == true)
        {
            secondaryWeapon.Weapon();
        }
    }

    public Vector2 GetEnemyPosition()
    {
        Collider2D[] Points = Physics2D.OverlapCircleAll(transform.position, radius, enemeyMask);

        if (Points.Length == 0)
        {
            return new Vector2(Random.Range(transform.position.x - 5f, transform.position.x + 5f), Random.Range(transform.position.y - 5f, transform.position.y + 5f));
        }

        for (int i = 0; i < Points.Length; i++)
        {
            if (ClostestEnemy == null)
            {
                ClostestEnemy = Points[i];

            }
            float ShortestDistance = Vector2.Distance(transform.position, Points[i].transform.position);
            float CurrentDistance = Vector2.Distance(transform.position, ClostestEnemy.transform.position);
            if (ShortestDistance <= CurrentDistance)
            {
                ClostestEnemy = Points[i];
            }
        }
        enemyHealth = ClostestEnemy.GetComponent<Testhealth>();
        return ClostestEnemy.transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
