using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class EnemyWithPathfinding : Enemy
{
    [Tooltip("This is the target that the opp chases down. Set it to Player in the inspector.")]
    public TriggerResponse playerTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the opp and player. Once the player walks into this detection radius, the opp will start chasing player down.
    public Vector2 range = new Vector2(8, 0);
    public SpriteRenderer foundPlayerSprite;

    [HideInInspector] public bool foundPlayer = false;
    protected NavMeshAgent agent;
    protected float boxColliderHeight;
    protected LayerMask groundLayer;
    protected LayerMask wallLayer;
    protected Transform target;

    public bool isWalkPointSet = false;

    
    public virtual void Start(){
        target = Player.Instance.transform;
        foundPlayerSprite.enabled = false;
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        playerTriggerResponse.onTriggerEnter2D = OnPlayerTriggerEnter2D;
        playerTriggerResponse.onTriggerExit2D = OnPlayerTriggerExit2D;
        groundLayer = LayerMask.GetMask("Ground");
        wallLayer = LayerMask.GetMask("Wall");

        agent.enabled = false;
        // These two lines of code are recommended to prevent innate agent rotation and up-axis updates. Without these lines of code, the agent can rotate the agent outside the 2D screen into 3D coordinates.
        agent.updateRotation = false;
        agent.updateUpAxis = false;
    }

    public virtual void Update()
    {
        if (isFrozen)
        {
            agent.enabled = false;
        }
        else if (foundPlayer)
        {
            agent.enabled = true;
            agent.SetDestination(target.position);
        }
        else
        {
            agent.enabled = false;
            Patrol();
        }
    }

    public void OnPlayerTriggerEnter2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer != layer) return;

        foundPlayer = true;
        foundPlayerSprite.enabled = true;
    }

    public void OnPlayerTriggerExit2D(Collider2D collider){
        int layer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer != layer) return;

        foundPlayer = false;
        foundPlayerSprite.enabled = false;
    }

    public void Patrol(){
        if (
            isWalkPointSet && (
                Vector3.Distance(transform.position, agent.destination) < 2 ||
                agent.velocity == Vector3.zero
            )
        )
        {
            isWalkPointSet = false;
        }
        else
        {
            RandomNavSphere(0.1f);
        }
    }

    public void RandomNavSphere (float distance) {
        float x = Random.Range(-range.x, range.x);
        float y = Random.Range(-range.y, range.y);

        agent.SetDestination(new Vector3(transform.position.x + x, transform.position.y + y, 0));

        NavMeshHit navHit;
        NavMesh.SamplePosition (agent.destination, out navHit, distance, NavMesh.AllAreas);

        if (navHit.hit)
        {
            isWalkPointSet = true;
        }
    }
}
