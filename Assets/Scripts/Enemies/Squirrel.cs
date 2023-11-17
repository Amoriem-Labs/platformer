using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squirrel : EnemyWithPathfinding
{
    public Vector2 force;
    private Animator animator;
    public float stunDuration; // This is the number of seconds to stun the player for when the squirrel collides with the player.
    public float damageAmount; // This is the amount of damage the squirrel does to the player when the squirrel collides with the player.

    void Start()
    {
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

    // When squirrel collides with player, pause player movement.
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(force);
            Player.DisablePlayerMovement();
            Player.TakeDamage(damageAmount);
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
