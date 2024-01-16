using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    public Rigidbody2D rb;
    public float walkspeed = 8;
    public float jumpspeed = 8;
    public float djumpspeed = 5;

    public bool isGrounded;
    public LayerMask groundLayer; 
    public LayerMask platformLayer;
    public Transform groundCheck; 
    public float groundCheckRadius = 0.2f;

    // Dash variables
    private bool canDash = true;
    public float dashPower = 100f;
    public float dashDuration = 0.7f;
    public float dashCooldown = 0.5f;
    public float lastDashTime = 0;
    public float dashPressTime = 0.2f;
    private bool isDashing;
    private float delayBetweenPresses = 0.25f;
    private bool pressedLeftFirstTime = false;
    private float lastPressedLeftTime;
    private bool pressedRightFirstTime = false;
    private float lastPressedRightTime;

    // Double jump variables
    public bool canDoubleJump = true;

    // Sprite variables
    public SpriteRenderer bodySprite;
    public SpriteRenderer sweaterSprite;
    public SpriteRenderer hairSprite;

    // Trail renderer variable
    private TrailRenderer tr;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        tr = GetComponent<TrailRenderer>();
    }

    private void Update()
    {
        if (isDashing){
            return;
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer);

        // Handle horizontal movement
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * walkspeed, rb.velocity.y);
        
        // Flip sprite according to movement
        if (rb.velocity.x != 0) { 
            bodySprite.flipX = rb.velocity.x < 0;
            sweaterSprite.flipX = rb.velocity.x < 0;
            hairSprite.flipX = rb.velocity.x < 0;
        }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (canDoubleJump)
            {
                DoubleJump();
            }
        }

        // Handle dashing left
        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow))
        {
            if (pressedLeftFirstTime) // we've already pressed the button a first time, we check if the 2nd time is fast enough to be considered a double-press
            {
                bool isDoublePress = Time.time - lastPressedLeftTime <= delayBetweenPresses;
                if (isDoublePress && canDash)
                {
                    StartCoroutine(Dash(-1)); // Dash left
                    pressedLeftFirstTime = false;
                }
            }
            else // we've not already pressed the button a first time
            {
                pressedLeftFirstTime = true; // we tell this is the first time
            }
            lastPressedLeftTime = Time.time;
        }

        // Handle dashing right
        else if (Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow))
        {
            if (pressedRightFirstTime) // we've already pressed the button a first time, we check if the 2nd time is fast enough to be considered a double-press
            {
                bool isDoublePress = Time.time - lastPressedRightTime <= delayBetweenPresses;
                if (isDoublePress && canDash)
                {
                    StartCoroutine(Dash(1)); // Dash right
                    pressedRightFirstTime = false;
                }
            }
            else // we've not already pressed the button a first time
            {
                pressedRightFirstTime = true; // we tell this is the first time
            }
            lastPressedRightTime = Time.time;
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpspeed);
        canDoubleJump = true; // Allow double jump after jumping off the ground
    }

    private void DoubleJump()
    {
        rb.velocity = new Vector2(rb.velocity.x, djumpspeed);
        canDoubleJump = false; // Disable double jump until we hit the ground again
    }

    private IEnumerator Dash(int direction)
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;
        rb.velocity = new Vector2(direction * dashPower, 0f);
        tr.emitting = true;
        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        tr.emitting = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }
}

