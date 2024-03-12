using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Enemy
{
    public Vector2 moveForce;
    public Vector2 hitForce;
    public float secWaitAfterCollision;
    public float energySap;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (isFrozen){
            float y_vel = rb.velocity.y;
            rb.velocity = new Vector2(0,y_vel);
        } else {
            rb.AddForce(moveForce);
        }
    }

    // When car first collides with player, disables player collisions for secWaitAfterCollision seconds between player and THIS CAR only and then re-enables player collisions
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            Player.DecreaseEnergy(energySap);
            if (rb.velocity.x > 0){
                Player.rb.AddForce(new Vector2(1, 1) * hitForce); // push player to the right if squirrel is moving right
            } else {
                Player.rb.AddForce(new Vector2(-1, 1) * hitForce); // push player to the left if squirrel is moving left
            }
        }
    }
}
