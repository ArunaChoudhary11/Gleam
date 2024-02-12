using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    public float Focus;
    public float intensity;
    public float Wavelength;
    public bool hasSecondWeapon;
    public SecondaryWeapon secondaryWeapon;

    [SerializeField] private GameObject AttackPoint;
    [SerializeField] private LayerMask enemeyMask;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AttackPower;
    [SerializeField] private float radius;
    private Collider2D ClostestEnemy;
    private TestHealth enemyHealth;
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left Mouse btn input
        {
            GameObject newAttackPoint = Instantiate(AttackPoint, transform.position, Quaternion.identity);
            Vector2 endPoint = GetEnemyPosition();
            PrimaryAttack attack = newAttackPoint.GetComponent<PrimaryAttack>();
            attack.GetValues(newAttackPoint, endPoint, enemyHealth, AttackPower, AttackSpeed);
        }
    }

    private void SecondWeapon()
    {
        if(hasSecondWeapon == true)
        {
            secondaryWeapon.Weapon();
        }
    }

    public Vector2 GetEnemyPosition()
    {
        Collider2D[] Points = Physics2D.OverlapCircleAll(transform.position, radius , enemeyMask);

        if(Points.Length == 0)
        {
            enemyHealth = null;
            // 
            return new Vector2(Random.Range(transform.position.x - 5f, transform.position.x + 5f), Random.Range(transform.position.y - 5f, transform.position.y + 5f));
        }

        for(int i = 0; i < Points.Length; i++)
        {
            if(ClostestEnemy == null)
            {
                ClostestEnemy = Points[i];

            }
            float ShortestDistance = Vector2.Distance(transform.position, Points[i].transform.position);
            float CurrentDistance = Vector2.Distance(transform.position, ClostestEnemy.transform.position);
            if(ShortestDistance <=  CurrentDistance)
            {
                ClostestEnemy = Points[i];
            }
        }
        enemyHealth = ClostestEnemy.GetComponent<TestHealth>();
        return ClostestEnemy.transform.position;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}