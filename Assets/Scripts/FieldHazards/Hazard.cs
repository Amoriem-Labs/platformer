using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float damageAmount; // This is the amount of damage the hazard does to the player.

    void OnTriggerEnter2D (Collider2D collider){
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == playerLayer){
            Player.TakeDamage(damageAmount);
            // play player damaged animation and sound effect
            collider.gameObject.transform.position = Player.lastGroundedPosition;
        }
    }
}
