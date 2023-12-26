using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class Opp : EnemyWithPathfinding
{
    public float secWaitAfterConvoEnds; // This is the number of seconds to wait after the conversation with the player ends before the Opp starts chasing the player again.
    public ConversationSO conversation; // This is a ScriptableObject that contains the conversation elements that the opp will trigger upon collision with player.
    public bool isThisOppTriggerOn = true; // This boolean determines whether opp triggers are turned on for this specific opp. If off, the player won't trigger a conversation upon colliding with this opp. If on, the player will trigger a conversation upon colliding with this opp. Upon RunTime, this boolean is set to true.
    public bool haveTalkedToAlready = false; // This boolean determines whether the player has already talked to this opp.
    
    public override void Update(){
        if (isFrozen){
            isThisOppTriggerOn = false;
            agent.enabled = false;
        } else {
            Invoke("ResumePathfinding", secWaitAfterConvoEnds);
        }
    }

    void Start()
    {
        base.Start();
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

    public override void ResumePathfinding(){
        agent.enabled = true;
        if (target == null){
            Patrol();
        } else {
            agent.SetDestination(target.position); // This method finds the shortest path to the target's position and makes the agent follow that path to move towards that position.
        }
        isThisOppTriggerOn = true;
    }
}
