using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject textBox; // This is a reference to the text box that will be triggered when the opp runs into the player.
    public Rigidbody2D rb;
    public TriggerResponse oppTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the player and opps.
    public TriggerResponse bookTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the player and books.
    public float bookKnockback; // This determines how much the player will be knocked back by when hit by a book.
    public float numSecondsFreeze; // This is the number of seconds to freeze an enemy.
    public float numSecondsShield; // This is how long the player's shield is activated in seconds.
    public SpriteRenderer shield; // This is the shield sprite.

    // Make sure that movement system has multiplying moveSpeed by Time.deltaTime to account for frame rates or using FixedUpdate

    void Start(){
        textBox.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        oppTriggerResponse.onTriggerEnter2D = OnOppDetectorTriggerEnter2D;
        oppTriggerResponse.onTriggerExit2D = OnOppDetectorTriggerExit2D;
        bookTriggerResponse.onTriggerEnter2D = OnBookDetectorTriggerEnter2D;
        bookTriggerResponse.onTriggerExit2D = OnBookDetectorTriggerExit2D;
        shield.enabled = false;
    }

    #region Shield functions.
    void Update(){
        if (Input.GetKeyDown(KeyCode.E) && !TextWriter.isWritingText){
            ActivateShield(numSecondsShield);
        }
    }

    void ActivateShield(float seconds){
        shield.enabled = true;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies){
            enemy.GetComponent<Enemy>().DisablePlayerCollisions();
        }
        Invoke("DeactivateShield", seconds);
    }

    void DeactivateShield(){
        shield.enabled = false;
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies){
            enemy.GetComponent<Enemy>().EnablePlayerCollisions();
        }
    }
    #endregion

    #region Freezing enemies.
    // Freezes all enemies on screen for numSecondsFreeze seconds. Remove this function in final production later. This function is purely for testing purposes.
    [ContextMenu("Freeze Enemies")]
    public void TestFreezeEnemies(){
        if (!TextWriter.isWritingText){
            FreezeEnemies(numSecondsFreeze);
        }
    }

    // Freezes all enemies on screen for X seconds.
    public void FreezeEnemies(float seconds){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies){
            enemy.GetComponent<Enemy>().isFrozen = true;
        }
        Invoke("UnfreezeEnemies", seconds);
    }

    // Unfreezes all enemies on screen
    public void UnfreezeEnemies(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies){
            enemy.GetComponent<Enemy>().isFrozen = false;
        }
    }
    #endregion

    #region Collider methods.
    // When an opp runs into the player, trigger a conversation that prevents the player from moving until the conversation is over.
    private void OnOppDetectorTriggerEnter2D(Collider2D collider)
    {
        int layer = LayerMask.NameToLayer("Opp");
        if (collider.gameObject.layer == layer)
        {
            Opp opp = collider.gameObject.GetComponent<Opp>();
            if (opp.isThisOppTriggerOn){
                TextWriter.ActivateConversation(opp.conversation);

                // TODO: WRITE CODE TO PAUSE PLAYER MOVEMENT
            }
        }
    }

    private void OnOppDetectorTriggerExit2D(Collider2D collider)
    {
        int layer = LayerMask.NameToLayer("Opp");
        if (collider.gameObject.layer == layer)
        {
            // Empty method for now. Fill in code later if you want code to be run when an opp leaves a player's hitbox.
        }
    }

    // When book collides with player, decrease player HP by 1 and destroy book.
    public void OnBookDetectorTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.tag == "Book"){
            Destroy(collider.gameObject);
            // If book is heading left, then have player be knocked back to the left. If book is heading right, then have player be knocked back to the right.
            if (collider.gameObject.GetComponent<Rigidbody2D>().velocity.x <= 0){
                rb.AddForce(new Vector2(-10, 5) * bookKnockback);
            } else {
                rb.AddForce(new Vector2(10, 5) * bookKnockback);
            }
            // TODO: Decrease player HP by 1 whenever health system is implemented.
        }
    }

    public void OnBookDetectorTriggerExit2D(Collider2D collider){
        if (collider.gameObject.tag == "Book"){
            // Empty method for now. Fill in code later if you want code to be run when an book leaves a player's hitbox.
        }
    }
    #endregion
}
