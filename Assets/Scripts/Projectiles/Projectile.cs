using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float damageAmount;
    public float knockBack;

    void OnTriggerEnter2D (Collider2D collider){
        int playerLayer = LayerMask.NameToLayer("Player");
        int groundLayer = LayerMask.NameToLayer("Ground");
        if (collider.gameObject.layer == playerLayer){
            Rigidbody2D playerRb = collider.GetComponent<Rigidbody2D>();
            Player.TakeDamage(damageAmount);
            // Knock back player based on which side of the player the projectile is on.
            if (collider.transform.position.x < transform.position.x){
                playerRb.AddForce(new Vector2(-10, 5) * knockBack);
            } else {
                playerRb.AddForce(new Vector2(10, 5) * knockBack);
            }
            Destroy(gameObject);
        }
        else if (collider.gameObject.layer == groundLayer){
            Destroy(gameObject);
        }
    }
}
