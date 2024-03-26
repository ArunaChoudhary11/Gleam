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
    [SerializeField] private PhotonShield photonShield;
    #endregion

    public bool canUse_SolDash;
    public bool canUse_SpeedDilation;
    public bool canUse_Refract;
    public bool canUse_NightVision;
    public bool canUse_PhotonShield;
    public bool CanAttack;
    public bool CanMove;
    public bool CanJump;
    public bool CanDash;
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
        playerMove = GetComponent<PlayerMove>();
    }
    private void InitializeComponents()
    {
        selectionDirection = GetComponent<SelectionDirection>();
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
        speedTimeDilation.enabled = canUse_SpeedDilation;
        nightVision.enabled = canUse_NightVision;
//        photonShield.enabled = canUse_PhotonShield;
        playerAttack.enabled = CanAttack;
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

        if(selectionDirection.canSelect == false) return RandomPoint();

        if(selectionDirection.CurrentSelectedEnemy == null) return RandomPoint();

        ClosestEnemy = selectionDirection.CurrentSelectedEnemy.GetComponent<Collider2D>();

        if(ClosestEnemy.gameObject.layer == 9) return RandomPoint();
        
        return ClosestEnemy.transform.position;
    }
    public Vector2 RandomPoint()
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
        float distance = Vector2.Distance(newPoint, (Vector2) transform.position);

        RaycastHit2D hit = Physics2D.Raycast (transform.position, direction, distance, ObstacleMask);
        
        if(hit.collider != null)
        {
            return hit.point;
        }

        return newPoint;
    }
}

/*  
    CanSelect - False:
    1. Random Point.

    CanSelect - True:
    1. Nearest Enemy
    2. Selected Enemy
    3. Random Point
*/