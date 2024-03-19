using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : EnemyWithPathfinding
{
    public bool isAttacking = false;
    public Vector2 hitForce;
    public TriggerResponse attackRangeTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the drone and player. Once the player walks into this detection radius, the drone will start throwing books at the player.
    public GameObject bookPrefab; // This is the book GameObject that the drone will throw.
    public float bookThrowSpeed; // This is the speed at which the drone will throw the book.
    public float timeBetweenBookThrows; // This is the time between each book throw.
    public float damageAmount;
    private float timeUntilNextBookThrow; // This is the time until the next book throw.

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
        attackRangeTriggerResponse.onTriggerEnter2D = OnAttackRangeEnter2D;
        attackRangeTriggerResponse.onTriggerExit2D = OnAttackRangeExit2D;
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");
        boxColliderHeight = GetComponent<BoxCollider2D>().size.y * transform.localScale.y; // Need to multiply by y-scale to get correct scaling relationship
    }

    // EY TODO
    //public override void Update()
    //{
    //    if (isFrozen){
    //        agent.enabled = false;
    //    } else if (isAttacking) {
    //        agent.enabled = false;
    //        timeUntilNextBookThrow += Time.deltaTime;
    //        if (timeUntilNextBookThrow >= timeBetweenBookThrows){
    //            AttackPlayer();
    //            timeUntilNextBookThrow = 0;
    //        }
    //    } else {
    //        ResumePathfinding();
    //    }
    //}

    public void AttackPlayer(){
        GameObject bookObj = Instantiate(bookPrefab, transform.position, Quaternion.identity);
        Vector3 directionToPlayer = (target.position - transform.position);
        directionToPlayer.Normalize();
        bookObj.GetComponent<Rigidbody2D>().velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * bookThrowSpeed;
    }

    // When player enters drone's attack radius, throw a book at the player.
    void OnAttackRangeEnter2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            isAttacking = true; // Turn off pathfinding while player is in attack range of drone to pause drone movement.
        }
    }

    void OnAttackRangeExit2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            isAttacking = false; // Turn back on pathfinding when player exits attack range of drone.
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            Player.TakeDamage(damageAmount);
            if (rb.velocity.x > 0){
                Player.rb.AddForce(new Vector2(1, 1) * hitForce); // push player to the right if squirrel is moving right
            } else {
                Player.rb.AddForce(new Vector2(-1, 1) * hitForce); // push player to the left if squirrel is moving left
            }
        }
    }
}
