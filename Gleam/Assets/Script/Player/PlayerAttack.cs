using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
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
    [SerializeField] private float radius;
    private Collider2D ClostestEnemy;
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
            attack.GetValues(newAttackPoint, endPoint, enemyHealth, AttackPower, AttackSpeed);
        }
    }

    // private void SecondWeapon()
    // {
    //     if(hasSecondWeapon == true)
    //     {
    //         secondaryWeapon.Weapon();
    //     }
    // }

    public Vector2 GetEnemyPosition()
    {
        fieldOfView.FindVisibleTargets();
        List<Collider2D> Points  = fieldOfView.visibleTargets;

        if(Points.Count == 0)
        {
            enemyHealth = null;
            int playerDirection = (int) transform.localScale.x;
            Vector2 newPoint = new Vector2(Random.Range(transform.position.x, transform.position.x + 5f * playerDirection), Random.Range(transform.position.y, transform.position.y + 5f));
            RaycastHit2D hit = Physics2D.Raycast (transform.position, newPoint, (newPoint - (Vector2) transform.position).sqrMagnitude, ObstacleMask);
            
            if(hit.collider != null)
            {
                return hit.point;
            }
            return newPoint;
        }

        for(int i = 0; i < Points.Count; i++)
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