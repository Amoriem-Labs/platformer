using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opp : MonoBehaviour
{
    public Transform target; // This is the target that the opp chases down. We set it to Player in the inspector, since the opp is meant to chase down the player.
    public float secWaitAfterConvoEnds; // This is the number of seconds to wait after the conversation with the player ends before the Opp starts chasing the player again.
    NavMeshAgent agent; // This is the NavMeshAgent component. It is needed for the SetDestination() method.
    public bool isOppTriggerOn = true; // This boolean determines whether opp triggers are turned on. If off, the player won't trigger a conversation upon colliding with opp. If on, the player will trigger a conversation upon colliding with opp. Upon RunTime, this boolean is set to true.

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // These two lines of code are recommended to prevent innate agent rotation and up-axis updates, as this can be handled by future animations.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    { 
        if (!TextWriter.isWritingText){ // This if statement basically says that if the TextWriter is not currently writing, then run the below code.
            Invoke("ResumePathfinding", secWaitAfterConvoEnds);
            agent.SetDestination(target.position); // This method finds the shortest path to the target's position and makes the agent follow that path to move towards that position.
        }
    }

    // This if method basically says that if the Opp runs into the Opp Detector, then immediately halt the NavMeshAgent's pathfinding, thus deactivating the "agent.SetDestination()" method.
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "OppDetector")
        {
            isOppTriggerOn = false;
            GetComponent<NavMeshAgent>().isStopped = true;
        }
    }

    private void ResumePathfinding(){
        isOppTriggerOn = true;
        GetComponent<NavMeshAgent>().isStopped = false; // This method immediately resumes the NavMeshAgent's pathfinding, and thus activates the "agent.SetDestination()" method.
    }
}
