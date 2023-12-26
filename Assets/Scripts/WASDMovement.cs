using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    public Rigidbody2D Playerbody;
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
    private float dashPower = 100f;
    private float dashDuration = 0.7f;
    private float dashCooldown = 0.5f;
    public float lastDashTime = 0;
    public float dashPressTime = 0.2f;
    private bool isDashing;

    // Double jump variables
    public bool canDoubleJump = true;

    // Sprite variables
    public SpriteRenderer bodySprite;
    public SpriteRenderer sweaterSprite;
    public SpriteRenderer hairSprite;

    private void Awake()
    {
        Playerbody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer);

        // Handle horizontal movement
        Playerbody.velocity = new Vector2(Input.GetAxis("Horizontal") * walkspeed, Playerbody.velocity.y);
        
        // Flip sprite according to movement
        if (Playerbody.velocity.x != 0) { 
            bodySprite.flipX = Playerbody.velocity.x < 0;
            sweaterSprite.flipX = Playerbody.velocity.x < 0;
            hairSprite.flipX = Playerbody.velocity.x < 0;
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

        // Handle dashing
        /* Kinda buggy for now so fix later
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && canDash)
        {
            StartCoroutine(Dash(-1)); // Dash left
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && canDash)
        {
            StartCoroutine(Dash(1)); // Dash right
        }*/
    }

    private void Jump()
    {
        Playerbody.velocity = new Vector2(Playerbody.velocity.x, jumpspeed);
        canDoubleJump = true; // Allow double jump after jumping off the ground
    }

    private void DoubleJump()
    {
        Playerbody.velocity = new Vector2(Playerbody.velocity.x, djumpspeed);
        canDoubleJump = false; // Disable double jump until we hit the ground again
    }

    private IEnumerator Dash(int direction)
    {
        canDash = false;
        Playerbody.velocity = new Vector2(dashPower * direction, Playerbody.velocity.y);
        yield return new WaitForSeconds(dashDuration);
        Playerbody.velocity = new Vector2(0, Playerbody.velocity.y); // Stop dashing
        yield return new WaitForSeconds(dashCooldown);
        canDash = true; // Reset dash
    }
}

