using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStateMachine
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private bool isFacingRight = true;
    [SerializeField] private LayerMask platformMask;
    [SerializeField] private bool canAddEdgeForce;

    [Header("Dashing")]
    public bool canDash = true;
    private bool isDashing;
    public float dashingPower = 16f;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [Header("Wall Slide")]
    private bool isWallSliding;
    private float wallSlidingSpeed = 1f;
    private bool iswallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingduration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);

    [Header("Components")]
    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isGrounded;
    private float coyoteTime;
    private float jumpBufferTime;
    private float jumpSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        ClampYVelocity();
        EdgeDetection();

        isGrounded = Physics2D.OverlapBox(groundCheck.position, Vector2.one, 0, groundLayer);

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Debug.Log("Dashing");
            StartCoroutine(Dash());
        }

        if (isDashing)
        {
            return;
        }

        horizontal = Input.GetAxisRaw("Horizontal");

        if(isGrounded)
        {
            coyoteTime = 0.2f;
        }
        else
        {
            coyoteTime -= Time.deltaTime;
        }

        if(Input.GetButtonDown ("Jump"))
        {
            jumpBufferTime = 0.2f;  
        }
        else
        {
            if(jumpBufferTime >= 0)
            {
                jumpBufferTime -= Time.deltaTime;
            }
        }

        if(jumpBufferTime > 0f && coyoteTime > 0f)
        {
            Jump();
        }

        if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
            coyoteTime = 0f;
        }

        WallSlide();
        WallJump();

        if(!iswallJumping)
        {
            Flip();
        }
    }
    private void ClampYVelocity()
    {
        if(rb.velocity.y <= -50f)
        {
            Vector2 velocity = rb.velocity;
            velocity.y = -50f;
            rb.velocity = velocity;
        }
    }
    private void Jump()
    {
        Vector2 velocity = rb.velocity;
        velocity.y = 0f;
        rb.velocity = velocity;
        jumpSpeed = Mathf.Sqrt(-2f * Physics2D.gravity.y * jumpingPower * 100000);
        rb.AddForce(Vector2.up * jumpSpeed);
        jumpBufferTime = 0f;
    }
    private void FixedUpdate()
    {
        if(!iswallJumping)
        {
            Movement();
        }
    }
    private void Movement()
    {
        Vector2 velocity = rb.velocity;
        velocity.x = horizontal * speed * Time.deltaTime * 1000;
        rb.velocity = velocity;
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
       if(IsWalled() && !isGrounded && horizontal != 0f )
        {
            isWallSliding = true;
            rb.velocity =  new Vector2(rb.velocity.x  , Mathf.Clamp(rb.velocity.y,-wallSlidingSpeed , float.MaxValue));
        }
    }
    private void WallJump()
    {
        if (isWallSliding)
        {
            iswallJumping = false;
            wallJumpingDirection = -transform.localScale.x;
            wallJumpingCounter = wallJumpingTime;

            CancelInvoke(nameof(StopWallJumping));
        } 
        else
        {
            wallJumpingCounter -= Time.deltaTime;
        }

        if(Input.GetButtonDown("Jump") && wallJumpingCounter > 0f)
        {
            isWallSliding = true;
            rb.velocity = new Vector2(wallJumpingDirection * wallJumpingPower.x, wallJumpingPower.y);
            wallJumpingCounter = 0f;

            if(transform.localScale.x != wallJumpingDirection)
            {
                isFacingRight = !isFacingRight;
                Vector3 localScale = transform.localScale;
                localScale.x *= -1f;
                transform.localScale = localScale;
            }

            Invoke(nameof(StopWallJumping), wallJumpingduration);
        }
    }
    private void StopWallJumping()
    {
        isWallSliding = false;
    }
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = Vector2.zero;
        rb.AddForce(Vector2.right * (transform.localScale.x * dashingPower* 1000), ForceMode2D.Impulse);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        canDash = true;
    }
    private void EdgeDetection()
    {
        Vector2 leftRayPosition = transform.position - new Vector3(0.45f, -0.4f, 0);
        Vector2 rightRayPosition = transform.position + new Vector3(0.45f, 0.4f, 0);
        Vector2 centerBoxPosition = transform.position + new Vector3(0, 0.4f, 0);
        Vector2 middleBoxPosition = transform.position;
        Vector2 bottomRayPosition = transform.position - new Vector3(1f, 0.4f, 0);

        Vector2 leftBoxSize = new Vector3(0.1f, 0.5f, 0);
        Vector2 rightBoxSize = new Vector3(0.1f, 0.5f, 0);
        Vector2 centerBoxSize = new Vector3(0.5f, 0.5f, 0);
        Vector2 middleBoxSize = new Vector3(1.2f, 0.6f, 0);
        Vector2 bottomBoxSize = new Vector3(1.3f, 0.2f, 0);

        Debug.DrawRay(leftRayPosition, Vector2.up, Color.green);
        Debug.DrawRay(rightRayPosition, Vector2.up, Color.green);
        Debug.DrawRay(bottomRayPosition, Vector2.right * 2, Color.green);

        RaycastHit2D leftEdge = Physics2D.BoxCast(leftRayPosition, leftBoxSize, 0, Vector2.up, 1.5f, platformMask);
        RaycastHit2D rightEdge = Physics2D.BoxCast(rightRayPosition, rightBoxSize, 0, Vector2.up, 1.5f, platformMask);
        RaycastHit2D TopEdge = Physics2D.BoxCast(centerBoxPosition, centerBoxSize, 0, Vector2.up, 1.5f, platformMask);
        RaycastHit2D MiddleEdge = Physics2D.BoxCast(middleBoxPosition, middleBoxSize, 0, Vector2.right, 2f, platformMask);
        RaycastHit2D bottomEdge = Physics2D.BoxCast(bottomRayPosition, bottomBoxSize, 0, Vector2.right, 2f, platformMask);

        if(isGrounded == false)
        {
            if(leftEdge.collider != null && rightEdge.collider != null || TopEdge.collider != null)
            {
                Debug.Log("Hitting Both");
                return;
            }

            if(bottomEdge.collider != null && horizontal != 0 && MiddleEdge.collider == null)
            {
                rb.AddForce(Vector2.up + Vector2.right * transform.localScale.x, ForceMode2D.Impulse);
                return;
            }

            if(rb.velocity.y > 0)
            {
                if(leftEdge.collider != null)
                {
                    rb.AddForce(Vector2.right * 200);
                    Debug.Log("Hitting Left");
                }

                if(rightEdge.collider != null)
                {
                    rb.AddForce(Vector2.right * -200);
                    Debug.Log("Hitting Right");
                }
            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(transform.position + new Vector3(0.45f, 0.4f, 0), new Vector3(0.1f, 0.5f, 0));
        Gizmos.DrawWireCube(transform.position - new Vector3(0.45f, -0.4f, 0), new Vector3(0.1f, 0.5f, 0));
        Gizmos.DrawWireCube(transform.position - new Vector3(0f, 0.45f, 0), new Vector3(1.2f, 0.1f, 0));

        Gizmos.color = Color.blue;
        Gizmos.DrawWireCube(transform.position, new Vector3(1.2f, 0.6f, 0));
        Gizmos.DrawWireCube(transform.position + new Vector3(0, 0.4f, 0), new Vector3(0.5f, 0.5f, 0));
    }
}