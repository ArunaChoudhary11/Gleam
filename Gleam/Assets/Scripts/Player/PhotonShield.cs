using UnityEngine;

public class PhotonShield : MonoBehaviour
{
    private float Angle;
    [HideInInspector] public bool isUsed;
    public bool canUse = true;
    private Vector3 ray1;
    private Vector3 ray2;
    [SerializeField] private GameObject projectile;

    public Color originalColor;
    public Color activatedColor;

    [Header("Attack")]
    [SerializeField] private float attackSpeed;
    [SerializeField] private float attackRaySize;
    [SerializeField] private float damage;

    public void Shield(Vector2 hitPoint, Vector2 hitNormal)
    {
        Angle = Random.Range(20f, 80f);
        ray1 = Quaternion.AngleAxis(Angle / 2, Vector3.forward) * hitNormal;
        Shoot(ray1);

        Angle = Random.Range(20f, 80f);
        ray2 = Quaternion.AngleAxis(-Angle / 2, Vector3.forward) * hitNormal;
        Shoot(ray2);
    }
    void Update()
    {
        if(canUse == true)
        {
            isUsed = true;
            canUse = false;
            Invoke(nameof(ResetShield), 1);
        }

        if(isUsed == true)
        {
            GetComponent<SpriteRenderer>().color = activatedColor;
        }
        else
        {
            GetComponent<SpriteRenderer>().color = originalColor;
        }
    }
    private void Shoot(Vector2 upDirection)
    {
        GameObject attackGO = Instantiate(projectile, transform.position, Quaternion.identity);
        attackGO.transform.up = upDirection;
        attackGO.GetComponent<EnemyTestAttack>().SetValues(attackSpeed, attackRaySize, damage);
    }
    private void ResetShield()
    {
        isUsed = false;
        Debug.Log("Reset");
    }
}