using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : PlayerStateMachine
{
    private float horizontal;
    [SerializeField] private float speed = 8f;
    [SerializeField] private float jumpingPower = 16f;
    private float acceleration = 7f;
    private float decceleration = 7f;
    private float velPower = 0.9f;
    private float frictionAmount = 0.2f;
    private bool isFacingRight = true;
    [SerializeField] private LayerMask platformMask;

    [Header("Dashing")]
    public bool canDash = true;
    private bool isDashing;
    public float dashingPower = 16f;
    public float dashingTime = 0.2f;
    private float dashingCooldown = 1f;

    [Header("Wall Movement")]
    [SerializeField] private float wallSlidingSpeed = 1f;
    private bool isWallSliding;
    private bool iswallJumping;
    private float wallJumpingDirection;
    private float wallJumpingCounter;
    [SerializeField] private float wallJumpingTime = 0.2f;
    [SerializeField] private float wallJumpingduration = 0.4f;
    [SerializeField] private Vector2 wallJumpingPower = new Vector2(8f, 16f);
    private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;

    [Header("Wall Jump")]
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    private bool isGrounded;
    private float coyoteTime;
    private float jumpBufferTime;
    private float jumpSpeed;
    [SerializeField] private bool isSliding;
    [SerializeField] private float SlidingSpeed;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }
    void Update()
    {
        ClampYVelocity();
        isGrounded = Physics2D.OverlapBox(groundCheck.position, Vector2.one, 0, groundLayer);
        horizontal = Input.GetAxisRaw("Horizontal");

        if (isDashing)
        {
            return;
        }
        
        EdgeDetection();
        Friction();

        if(Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            Debug.Log("Dashing");
            StartCoroutine(Dash());
        }


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

        if(isWallSliding == false)
        {
            if(jumpBufferTime > 0f && coyoteTime > 0f)
            {
                Jump();
            }

            if(Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
            {
                rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
                coyoteTime = 0f;
            }
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
        if(!iswallJumping && !isDashing)
        {
            Movement();
        }
    }
    private void Movement()
    {
        Vector2 velocity = rb.velocity;
        float targetSpeed = horizontal * speed * 10;
        float speedDifference = targetSpeed - velocity.x;
        float accelRate = (Mathf.Abs(targetSpeed) > 0f) ? acceleration : decceleration;
        float movement = Mathf.Pow(Mathf.Abs(speedDifference) * accelRate, velPower) * Mathf.Sign(speedDifference);
        
        rb.AddForce(movement * Vector2.right);
    }
    private void Friction()
    {
        if(isGrounded && horizontal == 0)
        {
            float amount = Mathf.Min(Mathf.Abs(rb.velocity.x), Mathf.Abs(frictionAmount));
            amount *= Mathf.Sign(rb.velocity.x);
            rb.AddForce(Vector2.right * -amount, ForceMode2D.Impulse);
        }
    }
    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }
    private void WallSlide()
    {
       if(IsWalled() && !isGrounded)
        {
            isWallSliding = true;
            rb.velocity =  new Vector2(rb.velocity.x, Mathf.Clamp(rb.velocity.y,-wallSlidingSpeed , float.MaxValue));
        }
        else
        {
            isWallSliding = false;
        }
    }
    private void Slide(Vector2 slideingDirection)
    {
        rb.AddForce(slideingDirection * SlidingSpeed * transform.localScale.x);
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
            iswallJumping = true;
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
        iswallJumping = false;
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
        rb.velocity = new Vector2(transform.localScale.x * dashingPower * 10, 0);
        yield return new WaitForSeconds(dashingTime);
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

        RaycastHit2D leftEdge = Physics2D.BoxCast(leftRayPosition, leftBoxSize, 0, Vector2.up, 3f, platformMask);
        RaycastHit2D rightEdge = Physics2D.BoxCast(rightRayPosition, rightBoxSize, 0, Vector2.up, 3f, platformMask);
        RaycastHit2D TopEdge = Physics2D.BoxCast(centerBoxPosition, centerBoxSize, 0, Vector2.up, 3f, platformMask);
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
                    rb.AddForce(Vector2.right * 50);
                    Debug.Log("Hitting Left");
                }

                if(rightEdge.collider != null)
                {
                    rb.AddForce(Vector2.right * -50);
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