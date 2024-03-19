using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Robot : Enemy
{
    public Vector2 knockbackForce;
    public float damageAmount;

    private Seek seekBehavior;

    private void Start()
    {
        base.Start();
        DisablePlayerCollisions();
        seekBehavior = GetComponent<Seek>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer != layer) return;

        // When robot collides with player, throw player in the air.

        // EY TODO: This code is kind of borked, probably because of SlowDownPlayer.
        //bool playerIsToLeft = seekBehavior.target.position.x < transform.position.x;
        //Player.rb.AddForce(
        //    knockbackForce *
        //    (playerIsToLeft ? new Vector2(-1, 1) : new Vector2(1, 1))
        //);


        Player.TakeDamage(damageAmount);
        ScoreManager.Instance.ResetMultiplier();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Z))
        {
            Player.rb.AddForce(
            knockbackForce
        );
        }
    }
}
