using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SleepDemon : Enemy
{
    public Vector2 hitForce;
    public float damageAmount;
    public float speed;
    private GameObject player;
    private Animator anim;

    void Start(){
        rb = GetComponent<Rigidbody2D>();
        thisCollider = GetComponent<Collider2D>();
        player = GameObject.FindGameObjectWithTag("Player");
        anim = GetComponent<Animator>();
    }

    // Sleep demon can bypass all obstacles and cannot be frozen.
    void FixedUpdate()
    {
        if (!GameManager.Instance.isGamePaused){
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed);
        }
    }

    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Player");
        if (collision.gameObject.layer == layer){
            anim.Play("Attack");
            Player.TakeDamage(damageAmount);
            if (rb.velocity.x > 0){
                Player.rb.AddForce(new Vector2(1, 1) * hitForce); // push player to the right if enemy is moving right
            } else {
                Player.rb.AddForce(new Vector2(-1, 1) * hitForce); // push player to the left if enemy is moving left
            }
            Invoke("PlayMovementAnim", 10f);
        }
    }

    void PlayMovementAnim(){
        anim.Play("Movement");
    }
}
