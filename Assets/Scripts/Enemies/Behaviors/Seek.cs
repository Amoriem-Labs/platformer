using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Seek : MonoBehaviour
{
    [Tooltip("When the robot detects the player, how much force it will apply towards the player")]
    public float forceTowardsPlayer;
    [Tooltip("The maximum left/right velocity")]
    public float maxVelocity;
    [Tooltip("The range in which the enemy will chase the player")]
    public float detectionRange;

    private Rigidbody2D rb;
    [System.NonSerialized] public Transform target;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        target = Player.Instance.transform;    
    }

    void FixedUpdate()
    {
        if (Vector3.Distance(target.position, transform.position) > detectionRange) return;

        bool playerIsToLeft = target.position.x < transform.position.x;
        rb.AddForce(new Vector2(playerIsToLeft ? -forceTowardsPlayer : forceTowardsPlayer, 0) * Time.fixedDeltaTime);

        // Set max X velocity
        rb.velocity = new Vector2(
            Mathf.Clamp(rb.velocity.x, -maxVelocity, maxVelocity),
            rb.velocity.y
        );
    }
}
