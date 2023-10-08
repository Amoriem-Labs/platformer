using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : Enemy
{
    public Vector2 force;
    private Rigidbody2D rb;
    private Collider2D thisCollider;
    public float secWaitAfterCollision;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<Collider2D>();
    }

    void FixedUpdate()
    {
        if (isFrozen){
            float y_vel = rb.velocity.y;
            rb.velocity = new Vector2(0,y_vel);
        } else {
            rb.AddForce(force);
        }
    }

    // When car first collides with player, disables player collisions for secWaitAfterCollision seconds between player and THIS CAR only and then re-enables player collisions
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            DisablePlayerCollisions();
            Invoke("EnablePlayerCollisions", secWaitAfterCollision);
        }
    }

    // Disables THIS car's collisions with player
    void DisablePlayerCollisions(){
        int currentlyExcludedLayersMask = thisCollider.excludeLayers.value; // This is the layer mask int for this car collider's currently excluded layers.
        int playerMask = LayerMask.GetMask("Player"); // This is the layer mask int for the "Player" layer.
        LayerMask newLayerMask = playerMask + currentlyExcludedLayersMask; // This is the layer mask (not an int) for the "Player" layer + all previously excluded layers.
        thisCollider.excludeLayers = newLayerMask; // Sets this collider's excludeLayers to the new layer mask.
    }

    // Enables THIS car's collisions with player only if this car's currently excluded layers contains the "Player" layer
    void EnablePlayerCollisions(){
        int playerLayer = LayerMask.NameToLayer("Player");

        if (LayerMaskExtensions.Includes(thisCollider.excludeLayers, playerLayer)){
            int currentlyExcludedLayersMask = thisCollider.excludeLayers.value; // This is the layer mask int for this car collider's currently excluded layers.
            int playerMask = LayerMask.GetMask("Player"); // This is the layer mask int for the "Player" layer.
            LayerMask newLayerMask = playerMask - currentlyExcludedLayersMask; // This is the layer mask (not an int) for all previously excluded layers - "Player" layer.
            thisCollider.excludeLayers = newLayerMask; // Sets this collider's excludeLayers to the new layer mask.
        }
    }
}
