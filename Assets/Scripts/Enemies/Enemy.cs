using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public bool isFrozen = false;
    [HideInInspector] public Rigidbody2D rb;
    [HideInInspector] public Collider2D thisCollider;

    protected void Start()
    {
        rb = GetComponent<Rigidbody2D>();

        // Find the first collider that's not a trigger
        var colliders = GetComponents<Collider2D>();
        foreach (Collider2D collider in colliders)
        {
            if (collider.isTrigger) continue;
            thisCollider = collider;
            break;
        }
    }

    // Disables THIS enemy's collisions with player
    [ContextMenu("Disable Player Collisions")]
    public void DisablePlayerCollisions(){
        int playerLayer = LayerMask.NameToLayer("Player");

        if (!LayerMaskExtensions.Includes(thisCollider.excludeLayers, playerLayer)){
            int currentlyExcludedLayersMask = thisCollider.excludeLayers.value; // This is the layer mask int for this enemy collider's currently excluded layers.
            int playerMask = LayerMask.GetMask("Player"); // This is the layer mask int for the "Player" layer.
            LayerMask newLayerMask = currentlyExcludedLayersMask + playerMask; // This is the layer mask (not an int) for the "Player" layer + all previously excluded layers.
            thisCollider.excludeLayers = newLayerMask; // Sets this collider's excludeLayers to the new layer mask.
        }
    }

    // Enables THIS enemy's collisions with player only if this enemy's currently excluded layers contains the "Player" layer
    [ContextMenu("Enable Player Collisions")]
    public void EnablePlayerCollisions(){
        int playerLayer = LayerMask.NameToLayer("Player");

        if (LayerMaskExtensions.Includes(thisCollider.excludeLayers, playerLayer)){
            int currentlyExcludedLayersMask = thisCollider.excludeLayers.value; // This is the layer mask int for this enemy collider's currently excluded layers.
            int playerMask = LayerMask.GetMask("Player"); // This is the layer mask int for the "Player" layer.
            LayerMask newLayerMask = currentlyExcludedLayersMask - playerMask; // This is the layer mask (not an int) for all previously excluded layers - "Player" layer.
            thisCollider.excludeLayers = newLayerMask; // Sets this collider's excludeLayers to the new layer mask.
        }
    }
}
