using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : EnemyWithPathfinding
{
    public Vector2 hitForce;
    private Animator animator;
    public float stunDuration; // This is the number of seconds to stun the player for when the squirrel collides with the player.
    public float energySap; // This is the amount of damage the squirrel does to the player when the squirrel collides with the player.

    #pragma warning disable 0114
    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<Collider2D>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = false;
        target = null;
        // These two lines of code are recommended to prevent innate agent rotation and up-axis updates. Without these lines of code, the agent can rotate the agent outside the 2D screen into 3D coordinates.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        playerTriggerResponse.onTriggerEnter2D = OnPlayerTriggerEnter2D;
        playerTriggerResponse.onTriggerExit2D = OnPlayerTriggerExit2D;
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");
        boxColliderHeight = GetComponent<BoxCollider2D>().size.y * transform.localScale.y; // Need to multiply by y-scale to get correct scaling relationship
        animator = GetComponent<Animator>();
    }
    #pragma warning restore 0114

    // When squirrel collides with player, pause player movement.
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            Player.DisablePlayerMovement();
            if (rb.velocity.x > 0){
                Player.rb.AddForce(new Vector2(1, 1) * hitForce); // push player to the right if squirrel is moving right
            } else {
                Player.rb.AddForce(new Vector2(-1, 1) * hitForce); // push player to the left if squirrel is moving left
            }
            Player.DecreaseEnergy(energySap);
            isFrozen = true;
            animator.enabled = false;
            Invoke("EnablePlayerAndSquirrelMovement", stunDuration);
        }
    }

    void EnablePlayerAndSquirrelMovement(){
        Player.EnablePlayerMovement();
        isFrozen = false;
        animator.enabled = true;
    }
}
