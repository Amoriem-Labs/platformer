using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tome : EnemyWithPathfinding
{
    public bool isDashing = false;
    public Vector2 hitForce;
    public Vector2 dashForce;
    public float timeBetweenDashes;
    public TriggerResponse attackRangeTriggerResponse; // Once the player walks into this attack range radius, the enemy will start dashing towards player.
    public float damageAmount;

    // Start is called before the first frame update
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
        attackRangeTriggerResponse.onTriggerEnter2D = OnAttackRangeEnter2D;
        attackRangeTriggerResponse.onTriggerExit2D = OnAttackRangeExit2D;
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");
        boxColliderHeight = GetComponent<BoxCollider2D>().size.y * transform.localScale.y; // Need to multiply by y-scale to get correct scaling relationship
    }

    IEnumerator Dash(){
        while (isDashing){
            rb.AddForce(dashForce); // force to dash towards player. Alternative method: change velocity
            yield return new WaitForSeconds(timeBetweenDashes);
        }
    }

    // When player enters tome's attack radius, tome starts dashing towards player.
    void OnAttackRangeEnter2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            isDashing = true; // Turn off pathfinding while player is in attack range of drone to pause drone movement.
            StartCoroutine(Dash());
        }
    }

    void OnAttackRangeExit2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            isDashing = false; // Turn back on pathfinding when player exits attack range of drone.
            StopCoroutine(Dash());
        }
    }

    // When tome collides with player, deal damage to player.
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            collision.gameObject.GetComponent<Rigidbody2D>().AddForce(hitForce);
            Player.TakeDamage(damageAmount);
        }
    }
}
