using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pendulum : MonoBehaviour
{
    public float energySap; // This is the amount of energy the hazard saps of the player.
    public bool walkableThroughAble; // This is whether or not the player can walk through the hazard without needing to respawn.
    public float rotation_rate;

    void Start(){
        StartCoroutine("Rotate");
    }

    IEnumerator Rotate(){
        while (true){
            float z_rot = transform.rotation.z;
            if (z_rot > -35f && z_rot < 35f){
                Vector3 vec = new Vector3(0, 0, z_rot += rotation_rate);
                Quaternion q = Quaternion.Euler(vec);
                transform.rotation = q;
                // idk how quaternion rotations work
                yield return new WaitForSeconds(0.05f);
            } else {
                rotation_rate *= -1;
            }
        }
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
