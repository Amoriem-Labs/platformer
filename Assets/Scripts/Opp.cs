using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Opp : MonoBehaviour
{
    public Transform target; // This is the target that the opp chases down. We set it to Player in the inspector, since the opp is meant to chase down the player.
    NavMeshAgent agent; // This is the NavMeshAgent component. It is needed for the SetDestination() method.

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();

        // These two lines of code are recommended to prevent innate agent rotation and up-axis updates, as this can be handled by future animations.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    void Update()
    {
        agent.SetDestination(target.position); // This method finds the shortest path to the target's position and makes the agent follow that path to move towards that position.
    }
}
