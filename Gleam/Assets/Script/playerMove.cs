using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

public class playerMove : MonoBehaviour
{

    private float horizontal;
    private float speed = 8f;
    private float jumpingPower = 16f;
    private bool isFacingRight = true;

    private bool candash = true;
    private bool isDashing;
    private float dashingPower = 16f;
    private float dashingTime = 0.2f;
    private float dashingCooldown = 1f;


    private bool isWallSliding;
    private float wallSlidingSpeed = 1f;


    private bool iswallJumping;
    private float wallJumpingDirection;
    private float wallJumpingTime = 0.2f;
    private float wallJumpingCounter;
    private float wallJumpingduration = 0.4f;
    private Vector2 wallJumpingPower = new Vector2(8f, 16f);



    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    [SerializeField] private TrailRenderer tr;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private LayerMask wallLayer;
    public bool isGrounded;



    // Start is called before the first frame update
    void Start()
    {
       
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);
        if (isDashing)
        {
            return;
        }
        horizontal = Input.GetAxisRaw("Horizontal");

        if(Input.GetButtonDown ("Jump") && isGrounded)
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);
        }
        if(Input.GetButtonUp("Jump") && rb.velocity.y>0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y*0.5f);
        }

        if(Input.GetKeyDown(KeyCode.LeftShift) && candash)
        {
            StartCoroutine(Dash());
        }
        wallslide();
        WallJump();


        if(!iswallJumping)
        {

            Flip();
        }

    }

   
    private void FixedUpdate()
    {

        if( !iswallJumping )
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);
        }
        
        

    }


    private bool IsWalled()
    {
        return Physics2D.OverlapCircle(wallCheck.position, 0.2f, wallLayer);
    }

    private void wallslide()
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
        candash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        rb.velocity = new Vector2(transform.localScale.x * dashingPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashingTime);
        tr.emitting = false;
        rb.gravityScale *= originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashingCooldown);
        candash = true;

    }
}
