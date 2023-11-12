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
    private bool canDoubleJump = true;

    // Sprite variables
    private SpriteRenderer Sprite;

    private void Awake()
    {
        Playerbody = GetComponent<Rigidbody2D>();
        Sprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);

        // IMPORTANT: Don't delete the part below bracketed in "===" symbols. There's a bug that if player runs into a wall, the rb.velocity gets reset to zero.
        // The below code fixes this bug. If you delete the below code, the player will be able to get stuck on walls, which is not good.
        // ====================================================================================================
        
        // Get the velocity
        Vector2 horizontalMove = Playerbody.velocity;
        // Don't use the vertical velocity
        horizontalMove.y = 0;

        float distance =  horizontalMove.magnitude * 10 * Time.fixedDeltaTime;
        // Normalize horizontalMove since it should be used to indicate direction
        horizontalMove.Normalize();
    
        // The layers for obstacles and walls
        LayerMask obstacleLayerMask = LayerMask.GetMask("Ground");
        LayerMask wallLayerMask = LayerMask.GetMask("Wall");

        // Check if the body's current velocity will result in a collision with obstacle or wall
        if(Physics2D.Raycast(transform.position, horizontalMove, distance, obstacleLayerMask) || Physics2D.Raycast(transform.position, horizontalMove, distance, wallLayerMask))
        {
            // If so, stop the movement
            Playerbody.velocity = new Vector2(0, Playerbody.velocity.y);
        } else {
            // Handle horizontal movement
            Playerbody.velocity = new Vector2(Input.GetAxis("Horizontal") * walkspeed, Playerbody.velocity.y);
        }
        // ====================================================================================================

        // Flip sprite according to movement
        if (Playerbody.velocity.x != 0) { Sprite.flipX = Playerbody.velocity.x < 0; }

        // Handle jumping
        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
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
        if ((Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.LeftArrow)) && canDash)
        {
            StartCoroutine(Dash(-1)); // Dash left
        }
        else if ((Input.GetKeyDown(KeyCode.D) || Input.GetKeyDown(KeyCode.RightArrow)) && canDash)
        {
            StartCoroutine(Dash(1)); // Dash right
        }
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

