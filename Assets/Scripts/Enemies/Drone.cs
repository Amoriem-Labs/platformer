using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Drone : Enemy
{
    public bool isAttacking = false;
    public Transform target; // This is the target that the drone chases down. We set it to Player in the inspector, since the opp is meant to chase down the player.
    private UnityEngine.AI.NavMeshAgent agent; // This is the NavMeshAgent component. It is needed for the SetDestination() method.
    public TriggerResponse playerTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the drone and player. Once the player walks into this detection radius, the drone will start chasing player down.
    public TriggerResponse attackRangeTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the drone and player. Once the player walks into this detection radius, the drone will start throwing books at the player.
    public GameObject bookPrefab; // This is the book GameObject that the drone will throw.
    public float bookThrowSpeed; // This is the speed at which the drone will throw the book.
    public float timeBetweenBookThrows; // This is the time between each book throw.
    private LayerMask groundLayer;
    private LayerMask wallLayer;
    private float boxColliderHeight;
    [SerializeField] private bool isWalkPointSet = false;
    [SerializeField] private Vector3 destPoint;
    public float x_range;
    public float y_range;

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

    void Update()
    {
        if (isFrozen || isAttacking){
            agent.enabled = false;
        } else {
            ResumePathfinding();
        }
    }

    IEnumerator AttackPlayer(){
        while (isAttacking){
            GameObject bookObj = Instantiate(bookPrefab, transform.position, Quaternion.identity);
            Vector3 directionToPlayer = (target.position - transform.position);
            directionToPlayer.Normalize();
            bookObj.GetComponent<Rigidbody2D>().velocity = new Vector2(directionToPlayer.x, directionToPlayer.y) * bookThrowSpeed;
            yield return new WaitForSeconds(timeBetweenBookThrows);
        }
    }

    #region Pathfinding and random walk functions
    private void ResumePathfinding(){
        agent.enabled = true;
        if (target == null){
            Patrol();
        } else {
            agent.SetDestination(target.position); // This method finds the shortest path to the target's position and makes the agent follow that path to move towards that position.
        }
    }

    private void OnPlayerTriggerEnter2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            target = collider.gameObject.transform;
            destPoint = collider.gameObject.transform.position;
        }
    }

    private void OnPlayerTriggerExit2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            target = null;
            agent.SetDestination(transform.position);
        }
    }

    private void Patrol(){
        if (isWalkPointSet){
            agent.SetDestination(destPoint);
            if (Vector3.Distance(transform.position, destPoint) < 2) isWalkPointSet = false;
        } else {
            SearchForDest();
        }
    }

    private void SearchForDest(){
        float x = Random.Range(-x_range, x_range);
        float y = Random.Range(-y_range, y_range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y + y, transform.position.z);

        // If the destination point is above the ground layer and if the destination point is not outside the walls and the player can still fit in the location set by the destination point without hitting a ceiling, then set the destination point.
        if (Physics2D.Raycast(destPoint, Vector3.down, groundLayer) && !Physics2D.Raycast(destPoint, Vector3.right, x_range, wallLayer) && !Physics2D.Raycast(destPoint, Vector3.left, x_range, wallLayer) && !Physics2D.Raycast(destPoint, Vector3.up, boxColliderHeight / 2 + 0.25f, groundLayer)){
            isWalkPointSet = true;
        }
    }
    #endregion

    // When player enters drone's attack radius, throw a book at the player.
    void OnAttackRangeEnter2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            isAttacking = true; // Turn off pathfinding while player is in attack range of drone to pause drone movement.
            StartCoroutine(AttackPlayer());
        }
    }

    void OnAttackRangeExit2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            isAttacking = false; // Turn back on pathfinding when player exits attack range of drone.
            StopCoroutine(AttackPlayer());
        }
    }
}
