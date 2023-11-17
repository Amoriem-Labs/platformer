using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyWithPathfinding : Enemy
{
    [HideInInspector] public Transform target; // This is the target that the opp chases down. We set it to Player in the inspector, since the opp is meant to chase down the player.
    [HideInInspector] public NavMeshAgent agent; // This is the NavMeshAgent component. It is needed for the SetDestination() method.
    public TriggerResponse playerTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the opp and player. Once the player walks into this detection radius, the opp will start chasing player down.
    [HideInInspector] public LayerMask groundLayer;
    [HideInInspector] public LayerMask wallLayer;
    [HideInInspector] public float boxColliderHeight;
    [SerializeField] private bool isWalkPointSet = false;
    [SerializeField] private Vector3 destPoint;
    public float x_range;
    public float y_range;
    

    public virtual void Update()
    { 
        if (isFrozen){
            agent.enabled = false;
        } else {
            ResumePathfinding();
        }
    }

    public virtual void ResumePathfinding(){
        agent.enabled = true;
        if (target == null){
            Patrol();
        } else {
            agent.SetDestination(target.position); // This method finds the shortest path to the target's position and makes the agent follow that path to move towards that position.
        }
    }

    public void OnPlayerTriggerEnter2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            target = collider.gameObject.transform;
            destPoint = collider.gameObject.transform.position;
        }
    }

    public void OnPlayerTriggerExit2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == layer){
            target = null;
            agent.SetDestination(transform.position);
        }
    }

    public void Patrol(){
        if (isWalkPointSet){
            agent.SetDestination(destPoint);
            if (Vector3.Distance(transform.position, destPoint) < 2) isWalkPointSet = false;
        } else {
            SearchForDest();
        }
    }

    public void SearchForDest(){
        float x = Random.Range(-x_range, x_range);
        float y = Random.Range(-y_range, y_range);

        destPoint = new Vector3(transform.position.x + x, transform.position.y + y, 0);

        // If the destination point is 1) above the ground layer and 2) won't hit an obstacle and 3) if the destination point is not outside the walls and 4) the player can still fit in the location set by the destination point without hitting a ceiling, then set the destination point.
        if ((Physics2D.Raycast(destPoint, Vector3.down, groundLayer).collider != null) && (Physics2D.Linecast(transform.position, destPoint, groundLayer).collider == null) && (Physics2D.Linecast(transform.position, destPoint, wallLayer).collider == null) && (Physics2D.Raycast(destPoint, Vector3.up, boxColliderHeight / 2 + 0.25f, groundLayer).collider == null)){
            isWalkPointSet = true;
        }
    }
}
