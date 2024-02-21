using System.Collections;
using UnityEngine;

public class Sol_Dash : MonoBehaviour
{
    public enum DashPhase { DISABLED , ACTIVATION , SELECTION , EXECUTION }
    public DashPhase dashPhase;
    private IEnumerator DashEnumerator;
    private SelectionDirection selectionDirection;
    public float dashTimer;
    private PlayerMove player;
    private bool isDashing;
    public bool canDash;
    public IEnumerator FreezeDelay;
    public IEnumerator StartDelay;
    [SerializeField] private LayerMask destructibleMask;
    
    void OnEnable()
    {
        player = GetComponent<PlayerMove>();
        selectionDirection = GetComponent<SelectionDirection>();
        ChangePhase(DashPhase.DISABLED);
    }
    void OnDisable()
    {
        ChangePhase(DashPhase.DISABLED);
    }
    void Update()
    {
        SetDashPhase();
    }
    private void CurrentPhase()
    {
        switch(dashPhase)
        {
            case DashPhase.ACTIVATION:
                Activating();
            break;

            case DashPhase.SELECTION:
                Selecting();
            break;

            case DashPhase.EXECUTION:
                Executing();
            break;

            default:
                Reset();
            break;
        }
    }
    private void ChangePhase(DashPhase phase)
    {
        dashPhase = phase;
        CurrentPhase();
    }
    private void Activating()
    {
        player.speed = 10;
        player.jumpingPower = 4;
        player.decceleration = 20;

        isDashing = true;

        Debug.Log("Sol_Dash Activation Phase");
    }
    private void Selecting()
    {
        selectionDirection.canSelect = true;

        if(FreezeDelay != null) StopCoroutine(FreezeDelay);

        StartDelay = TimeFreeze(4);

        StartCoroutine(StartDelay);

        Debug.Log("Sol_Dash Selection Phase");
    }
    private void Executing()
    {
        selectionDirection.canSelect = false;

        StopCoroutine(StartDelay);

        Time.timeScale = 1;

        Transform enemy = selectionDirection.CurrentSelectedEnemy;
        if(enemy != null) MoveTowards(enemy.position);

        FreezeDelay = TimeFreeze(0.1f);
        StartCoroutine(FreezeDelay);

        Debug.Log("Sol_Dash Execution Phase");
    }
    private void Reset()
    {
        player.speed = 1.5f;
        player.jumpingPower = 2;
        player.decceleration = 10;

        isDashing = false;

        if (DashEnumerator != null) StopCoroutine(DashEnumerator);
        if (FreezeDelay != null) StopCoroutine(FreezeDelay);
        if (StartDelay != null) StopCoroutine(StartDelay);

        FreezeDelay = null;
        StartDelay = null;
        DashEnumerator = null;

        Time.timeScale = 1;

        Debug.Log("Sol_Dash is disabled");
    }
    private void SetDashPhase()
    {
        if(canDash == true)
        {
            DashEnumerator = Dash(dashTimer);
            StartCoroutine(DashEnumerator);
            canDash = false;
        }

        if(isDashing == true)
        {
            if(Input.GetKeyDown(KeyCode.L))
            {
                if(dashPhase == DashPhase.SELECTION)
                {
                    ChangePhase(DashPhase.EXECUTION);
                    return;
                }

                ChangePhase(DashPhase.SELECTION);
            }
        }
    }

    private IEnumerator Dash(float duration)
    {
        ChangePhase(DashPhase.ACTIVATION);
        yield return new WaitForSeconds(duration);
        ChangePhase(DashPhase.DISABLED);
    }

    private IEnumerator TimeFreeze(float duration)
    {
        Time.timeScale = 0.1f;
        yield return new WaitForSecondsRealtime(duration);
        ChangePhase(DashPhase.ACTIVATION);
        Time.timeScale = 1;
    }

    private void MoveTowards(Vector2 target)
    {
        transform.position = target;

        Collider2D[] nearbyDestructibles = Physics2D.OverlapCircleAll(transform.position, 3f, destructibleMask);

        if(nearbyDestructibles.Length != 0)
        {
            foreach(Collider2D obj in nearbyDestructibles)
            {
                Destroy(obj.gameObject);
            }
        }
    }
}