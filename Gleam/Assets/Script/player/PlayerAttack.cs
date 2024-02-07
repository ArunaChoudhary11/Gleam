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
    public GameObject AttackPoint;
    public float AttackSpeed;
    public bool isAttacking;

    Vector2 endPoint;




    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {

        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            AttackPoint.transform.position = transform.position;
            endPoint = new Vector2(Random.Range(transform.position.x -5f , transform.position.x+5f) , Random.Range(transform.position.y - 5f, transform.position.y + 5f));
            isAttacking = true;
        }

        if(isAttacking)
        {
            Attack(endPoint);
        }

        float AttackDistance = Vector3.Distance(AttackPoint.transform.position , endPoint);
        if (AttackDistance <= 0.1f)
        {
            isAttacking = false;
        }
            
    }

    private void Attack(Vector2 endPoint)
    {
        Debug.Log("Attack");
        AttackPoint.transform.position = Vector2.Lerp(AttackPoint.transform.position, endPoint, AttackSpeed * Time.deltaTime );
    }

    private void SecondWeapon()
    {
        if(hasSecondWeapon == true)
        {
            secondaryWeapon.Weapon();
        }
    }  
}