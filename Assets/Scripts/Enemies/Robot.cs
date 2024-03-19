using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : EnemyWithPathfinding
{
    public Vector2 force;
    public float damageAmount;

    #pragma warning disable 0114
    // Start is called before the first frame update
    void Start()
    {
        base.Start();
        rb = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<Collider2D>();
        boxColliderHeight = GetComponent<BoxCollider2D>().size.y * transform.localScale.y; // Need to multiply by y-scale to get correct scaling relationship
    }
    #pragma warning restore 0114

    // When robot collides with player, throw player in the air.
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer != layer) return;
        Player.rb.AddForce(
            new Vector2(rb.velocity.x > 0 ? 1 : -1, 1) * force
        );
        Player.TakeDamage(damageAmount);
        ScoreManager.Instance.ResetMultiplier();
    }
}
