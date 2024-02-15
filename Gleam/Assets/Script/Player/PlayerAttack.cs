using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
    // public float Focus;
    // public float intensity;
    // public float Wavelength;
    // public bool hasSecondWeapon;
    // public SecondaryWeapon secondaryWeapon;
    [SerializeField] private GameObject AttackPoint;
    [SerializeField] private LayerMask enemeyMask;
    [SerializeField] private LayerMask ObstacleMask;
    [SerializeField] private float AttackSpeed;
    [SerializeField] private float AttackPower;
    private Collider2D ClosestEnemy;
    private TestHealth enemyHealth;
    private FieldOfView fieldOfView;
    
    void Start()
    {
        fieldOfView = GetComponent<FieldOfView>();
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0)) // Left Mouse btn input
        {
            GameObject newAttackPoint = Instantiate(AttackPoint, transform.position, Quaternion.identity);
            Vector2 endPoint = GetEnemyPosition();
            PrimaryAttack attack = newAttackPoint.GetComponent<PrimaryAttack>();
            attack.SetValues(endPoint, enemyHealth, AttackPower, AttackSpeed);
        }
    }

    // private void SecondWeapon()
    // {
    //     if(hasSecondWeapon == true)
    //     {
    //         secondaryWeapon.Weapon();
    //     }
    // }

    public int playerDirection;
    public Vector2 direction;
    public Vector2 GetEnemyPosition()
    {
        fieldOfView.FindVisibleTargets();
        List<Collider2D> Points  = fieldOfView.visibleTargets;

        if(Points.Count == 0)
        {
            enemyHealth = null;
            playerDirection = (int) transform.localScale.x;
            Vector2 newPoint = new();

            if(playerDirection > 0)
            {
                newPoint.x = Random.Range(transform.position.x, transform.position.x + 7f);
            }
            else
            {
                newPoint.x = Random.Range(transform.position.x - 7f, transform.position.x);
            }

            newPoint.y = Random.Range(transform.position.y, transform.position.y + 7f);

            direction = newPoint - (Vector2) transform.position;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast (transform.position, direction, distance, ObstacleMask);
            
            if(hit.collider != null)
            {
                Debug.DrawLine(transform.position, hit.point, Color.grey, 1);
                return hit.point;
            }

            Debug.DrawLine(transform.position, newPoint, Color.cyan, 1);
            return newPoint;
        }

        for(int i = 0; i < Points.Count; i++)
        {
            if(ClosestEnemy == null)
            {
                ClosestEnemy = Points[i];
            }
            float ShortestDistance = Vector2.Distance(transform.position, Points[i].transform.position);
            float CurrentDistance = Vector2.Distance(transform.position, ClosestEnemy.transform.position);
            if(ShortestDistance <=  CurrentDistance)
            {
                ClosestEnemy = Points[i];
            }
        }
        enemyHealth = ClosestEnemy.GetComponent<TestHealth>();
        return ClosestEnemy.transform.position;
    }
}