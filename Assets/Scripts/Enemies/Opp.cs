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
    private Rigidbody2D rb; // This is the Rigidbody2D component of the opp.
    public bool isThisOppTriggerOn = true; // This boolean determines whether opp triggers are turned on for this specific opp. If off, the player won't trigger a conversation upon colliding with this opp. If on, the player will trigger a conversation upon colliding with this opp. Upon RunTime, this boolean is set to true.
    public ConversationSO conversation; // This is a ScriptableObject that contains the conversation elements that the opp will trigger upon collision with player.

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        rb = GetComponent<Rigidbody2D>();
        // These two lines of code are recommended to prevent innate agent rotation and up-axis updates, as this can be handled by future animations.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    { 
        if (isFrozen){
            isThisOppTriggerOn = false;
            agent.enabled = false;
        } else {
            agent.enabled = true;
            Invoke("ResumePathfinding", secWaitAfterConvoEnds);
        }
    }

    private void ResumePathfinding(){
        if (agent.enabled) {
            agent.SetDestination(target.position); // This method finds the shortest path to the target's position and makes the agent follow that path to move towards that position.
        }
        isThisOppTriggerOn = true;
    }
}
