using System.Collections.Generic;
using UnityEngine;
public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance;

    #region Abilities
    private Sol_Dash soL_Dash;
    private PlayerMove playerMove;
    private SelectionDirection selectionDirection;
    private SpeedTimeDilation speedTimeDilation;
    [HideInInspector] public Refract refract;
    private NightVision nightVision;
    private PlayerAttack playerAttack;
    private SecondaryWeapon secondaryWeapon;
    [SerializeField] private PhotonShield photonShield;
    #endregion
    
    private FieldOfView fieldOfView;

    public bool canUse_SolDash;
    public bool canUse_SpeedDilation;
    public bool canUse_Refract;
    public bool canUse_NightVision;
    public bool canUse_PhotonShield;
    public bool canUse_PrimaryAttack;
    public bool canUse_SecondaryAttack;
    public bool CanMove;
    public bool IsInvincible;
    
    [HideInInspector] public Collider2D ClosestEnemy;    

    [SerializeField] private LayerMask ObstacleMask;
    void Awake()
    {
        if(Instance == null) Instance = this;
        else Destroy(this);
    }
    private void InitializeAbilities()
    {
        soL_Dash = GetComponent<Sol_Dash>();
        speedTimeDilation = GetComponent<SpeedTimeDilation>();
        refract = GetComponent<Refract>();
        nightVision = GetComponent<NightVision>();
        playerAttack = GetComponent<PlayerAttack>();
        secondaryWeapon = GetComponent<SecondaryWeapon>();
        playerMove = GetComponent<PlayerMove>();
    }
    private void InitializeComponents()
    {
        selectionDirection = GetComponent<SelectionDirection>();
        fieldOfView = GetComponent<FieldOfView>();
    }
    void Start()
    {
        InitializeAbilities();
        InitializeComponents();
    }
    void Update()
    {
        SetAbilities();
    }
    private void SetAbilities()
    {
        playerMove.enabled = CanMove;
        soL_Dash.enabled = canUse_SolDash;
        selectionDirection.enabled = canUse_SolDash;
        speedTimeDilation.enabled = canUse_SpeedDilation;
        nightVision.enabled = canUse_NightVision;
        photonShield.enabled = canUse_PhotonShield;
        playerAttack.enabled = canUse_PrimaryAttack;
        secondaryWeapon.enabled = canUse_SecondaryAttack;
    }
    public float health;
    public void TakeDamage(float _damage)
    {
        if(IsInvincible == true) return;

        health -= _damage;
        if (health <= 0)
        {
            Destroy(gameObject);
        }
    }
    public Vector2 GetClosestPoint()
    {
        ClosestEnemy = null;
        fieldOfView.FindVisibleTargets(transform.position);
        List<Collider2D> Points  = fieldOfView.visibleTargets;

        if(Points.Count == 0)
        {
            int playerDirection = (int) transform.localScale.x;
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

            Vector2 direction = newPoint - (Vector2) transform.position;
            float distance = direction.magnitude;

            RaycastHit2D hit = Physics2D.Raycast (transform.position, direction, distance, ObstacleMask);
            
            if(hit.collider != null)
            {
                return hit.point;
            }

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

        return ClosestEnemy.transform.position;
    }
}