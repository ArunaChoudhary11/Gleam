using UnityEngine;

public class PrimaryAttack : MonoBehaviour
{
    private GameObject _attackPoint;
    private Vector2 _endPoint;
    private TestHealth _enemyHealth;
    private float _attackPower;
    private float _attackSpeed;
    private bool attacking;
    public void GetValues(GameObject attackPoint, Vector2 endPoint, TestHealth enemyHealth, float power, float attackSpeed)
    {
        _attackPoint = attackPoint;
        _endPoint = endPoint;
        _enemyHealth = enemyHealth;
        _attackPower = power;
        _attackSpeed = attackSpeed;
        attacking = true;
    }

    void Update()
    {
        if(attacking == true)
        {
            Debug.Log("Attack");
            _attackPoint.transform.position = Vector2.Lerp(_attackPoint.transform.position, _endPoint, _attackSpeed * Time.deltaTime);
            
            float AttackDistance = Vector3.Distance(_attackPoint.transform.position, _endPoint);

            if (AttackDistance <= 0.1f)
            {
                if(_enemyHealth != null)
                {
                    _enemyHealth.TakeDamage(_attackPower);
                }
                
                Destroy(_attackPoint);
            }
        }
    }
}