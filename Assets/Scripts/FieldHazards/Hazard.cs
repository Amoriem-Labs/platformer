using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    public float energySap; // This is the amount of energy the hazard saps of the player.
    public bool walkableThroughAble; // This is whether or not the player can walk through the hazard without needing to respawn.

    void OnTriggerEnter2D (Collider2D collider){
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == playerLayer){
            Player.DecreaseEnergy(energySap);
            // play player damaged animation and sound effect
            if (!walkableThroughAble){
                collider.gameObject.transform.position = Player.lastGroundedPosition;
                // for weed fumes, player should turn green temporarily
            }
        }
    }
}
