using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float damageAmount; // This is the amount of damage the hazard does to the player.
    public bool walkableThroughAble; // This is whether or not the player can walk through the hazard without needing to respawn.

    void OnTriggerEnter2D (Collider2D collider){
        if (!GameManager.Instance.isGamePaused){
            int playerLayer = LayerMask.NameToLayer("Player");
            if (collider.gameObject.layer == playerLayer){
                Player.TakeDamage(damageAmount);
                // play player damaged animation and sound effect
                if (!walkableThroughAble){
                    collider.gameObject.transform.position = Player.lastGroundedPosition;
                    // for weed fumes, player should turn green temporarily
                }
            }
        }
    }
}
