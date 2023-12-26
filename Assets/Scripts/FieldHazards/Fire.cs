using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Fire : MonoBehaviour
{
    public float radiusToSearch; // This is the radius to search for tiles to set on fire.
    public float secondsUntilFireSpread; // This is the number of seconds until the fire spreads to another tile.
    private float timeUntilFireSpread = 0; // This is the time until the fire spreads to another tile.
    public float timePlayerIsOnFire;
    private int playerLayer;
    private int groundLayer;
    private int platformLayer;

    void Start(){
        playerLayer = LayerMask.NameToLayer("Player");
        groundLayer = LayerMask.NameToLayer("Ground");
        platformLayer = LayerMask.NameToLayer("Platform");
    }

    public void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.layer == playerLayer){
            Player player = collider.gameObject.GetComponent<Player>();
            player.SetPlayerOnFire();
        }
    }

    public void OnTriggerExit2D(Collider2D collider){
        if (collider.gameObject.layer == playerLayer){
            Player player = collider.gameObject.GetComponent<Player>();
            player.Invoke("ExtinguishPlayerFire", timePlayerIsOnFire);
        }
    }

    // implementation for fire spreading:
    // 1) search in a radius of say 5 tiles. if the closest tile within this radius (as detected by tag "Platform" or "Ground") is not on fire, then set this tile on fire. if the closest tile is already on fire, do not set the tile on fire.
    // 2) iterate over this process for all tiles within this radius.
    // * if there is no tile within this radius that is not on fire, then do nothing.
    void Update(){
        timeUntilFireSpread += Time.deltaTime;
        if (timeUntilFireSpread >= secondsUntilFireSpread){
            SpreadFire();
            timeUntilFireSpread = 0;
        }
    }

    void SpreadFire(){
        var nearestTile = Physics2D.OverlapCircle(transform.position, radiusToSearch, groundLayer) || Physics2D.OverlapCircle(transform.position, radiusToSearch, platformLayer);
        Debug.Log(nearestTile.GetType());

        //if (nearestTile != null){
        //    // Draw a line from the fire to the nearest tile
        //    Debug.DrawLine(transform.position, nearestTile.transform.position, Color.red);
        //}
    }
}
