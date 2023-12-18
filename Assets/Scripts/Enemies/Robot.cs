using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : EnemyWithPathfinding
{
    public Vector2 force;
    public float damageAmount;

    // Start is called before the first frame update
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
    }

    // When robot collides with player, throw player in the air.
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            if (rb.velocity.x > 0){
                Player.rb.AddForce(new Vector2(1, 1) * force); // push player to the right if squirrel is moving right
            } else {
                Player.rb.AddForce(new Vector2(-1, 1) * force); // push player to the left if squirrel is moving left
            }
            Player.TakeDamage(damageAmount);
        }
    }
}
