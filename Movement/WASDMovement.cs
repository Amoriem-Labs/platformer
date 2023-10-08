using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WASDMovement : MonoBehaviour
{
    //Dash movement still in progress. I added a ground check to make sure we can implement double jump easier later.
    public Rigidbody2D Playerbody;
    public float walkspeed = 8;
    public float jumpspeed = 7;

    public bool isGrounded;

   

    // dashing vars
    private bool CanDash = true; 
    private bool isDashing;
    private float DashingPower = 24f;
    private float Dashingtime = 0.24f;

    private float DashingCoolDown = 0.5f;

    

    private void Awake()
    {
        Playerbody = GetComponent<Rigidbody2D>();

    }

    // Update is called once per frame
    void Update()
    {
        
        Playerbody.velocity = new Vector2(Input.GetAxis("Horizontal") * walkspeed, Playerbody.velocity.y);

        if ((Input.GetKey(KeyCode.W)) || (Input.GetKey(KeyCode.UpArrow)) && isGrounded){
            Playerbody.velocity = new Vector2 (Playerbody.velocity.x , jumpspeed);
        }

        //if (Input.)

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit2D(Collision2D other)
    {
        if(other.gameObject.CompareTag("ground"))
        {
            isGrounded = false;
        }
    }

    /*private IEnumerator Dash()
    {
        CanDash = false;
        isDashing = true;
        float originalGravity = Playerbody.gravityScale;
        Playerbody.gravityScale = 0f;
        Playerbody.velocity = new Vector2(transform.localScale.x * DashingPower, Playerbody.velocity.y);
        yield return new WaitForSeconds(Dashingtime);
        Playerbody.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(DashingCoolDown);
        CanDash = true;
    
    }*/

    
}
