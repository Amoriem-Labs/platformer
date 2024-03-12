using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float energySap; // This is the amount of energy the hazard saps of the player.
    public bool walkableThroughAble; // This is whether or not the player can walk through the hazard without needing to respawn.
    public float rotation_rate;
    public Vector3 rotation_dir = new Vector3(0, 0, 1);
    private float rotation_z;
    private bool movingRight = true;
    private bool movingLeft = false;

    void Update(){
        if(transform.eulerAngles.z <= 180f)
        {
            rotation_z = transform.eulerAngles.z;
        } else {
            rotation_z = transform.eulerAngles.z - 360f;
        }
        if (rotation_z < -35f) {
            movingRight = true;
            movingLeft = false;
        }
        if (rotation_z > 35f) {
            movingRight = false;
            movingLeft = true;
        }
        if (movingRight) {transform.Rotate(rotation_rate * rotation_dir * Time.deltaTime); }
        if (movingLeft) {transform.Rotate(rotation_rate * rotation_dir*-1 * Time.deltaTime); }
    }

    void OnTriggerEnter2D (Collider2D collider){
        int playerLayer = LayerMask.NameToLayer("Player");
        if (collider.gameObject.layer == playerLayer){
            Player.DecreaseEnergy(energySap);
            // play player damaged animation and sound effect
            if (!walkableThroughAble){
                collider.gameObject.transform.position = Player.lastGroundedPosition;
            }
        }
    }
}
