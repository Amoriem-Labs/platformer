using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Opp : Enemy
{
    public Transform target; // This is the target that the opp chases down. We set it to Player in the inspector, since the opp is meant to chase down the player.
    public float secWaitAfterConvoEnds; // This is the number of seconds to wait after the conversation with the player ends before the Opp starts chasing the player again.
    private NavMeshAgent agent; // This is the NavMeshAgent component. It is needed for the SetDestination() method.
    public bool isThisOppTriggerOn = true; // This boolean determines whether opp triggers are turned on for this specific opp. If off, the player won't trigger a conversation upon colliding with this opp. If on, the player will trigger a conversation upon colliding with this opp. Upon RunTime, this boolean is set to true.
    public ConversationSO conversation; // This is a ScriptableObject that contains the conversation elements that the opp will trigger upon collision with player.
    public TriggerResponse playerTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the opp and player. Once the player walks into this detection radius, the opp will start chasing player down.
    private LayerMask groundLayer;
    private LayerMask wallLayer;
    private float boxColliderHeight;
    [SerializeField] private bool isWalkPointSet = false;
    [SerializeField] private Vector3 destPoint;
    public float x_range;
    public float y_range;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<Collider2D>();
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        agent.enabled = false;
        target = null;
        // These two lines of code are recommended to prevent innate agent rotation and up-axis updatesWithout these lines of code, the agent can rotate the agent outside the 2D screen into 3D coordinates.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        playerTriggerResponse.onTriggerEnter2D = OnPlayerTriggerEnter2D;
        playerTriggerResponse.onTriggerExit2D = OnPlayerTriggerExit2D;
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");
        boxColliderHeight = GetComponent<BoxCollider2D>().size.y * transform.localScale.y; // Need to multiply by y-scale to get correct scaling relationship
    }

    void Update()
    { 
        if (isFrozen){
            isThisOppTriggerOn = false;
            agent.enabled = false;
        } else {
            Invoke("ResumePathfinding", secWaitAfterConvoEnds);
        }
    }

    private void ResumePathfinding(){
        agent.enabled = true;
        if (target == null){
            Patrol();
        } else {
            agent.SetDestination(target.position); // This method finds the shortest path to the target's position and makes the agent follow that path to move towards that position.
        }
        isThisOppTriggerOn = true;
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
}
