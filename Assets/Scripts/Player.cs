using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject textBox; // This is a reference to the text box that will be triggered when the opp runs into the player.
    public static Rigidbody2D rb;
    public TriggerResponse oppTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the player and opps.
    public float numSecondsFreeze; // This is the number of seconds to freeze an enemy.
    public float numSecondsShield; // This is how long the player's shield is activated in seconds.
    public SpriteRenderer shield; // This is the shield sprite.
    public static WASDMovement movement; // Script for player movement
    private static SpriteRenderer playerSprite; // This is the player's sprite renderer.
    public static float secWaitAfterCollision = 2.5f; // This is the number of seconds to wait after a collision before re-enabling player collisions.

    // Make sure that movement system has multiplying moveSpeed by Time.deltaTime to account for frame rates or using FixedUpdate

    void Start(){
        textBox.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        oppTriggerResponse.onTriggerEnter2D = OnOppDetectorTriggerEnter2D;
        oppTriggerResponse.onTriggerExit2D = OnOppDetectorTriggerExit2D;
        shield.enabled = false;
        movement = GetComponent<WASDMovement>();
        playerSprite = GetComponent<SpriteRenderer>();
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
                if (opp.haveTalkedToAlready){
                    TextWriter.timePerCharacter = TextWriter.originalTimePerCharacter / 2;
                    TimeManager.minuteToRealTime = TimeManager.originalMinuteToRealTime / 2;
                }
                TextWriter.ActivateConversation(opp.conversation);
                opp.haveTalkedToAlready = true;
                DisablePlayerMovement();
                rb.velocity = Vector2.zero;
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

    public void OnBookDetectorTriggerExit2D(Collider2D collider){
        if (collider.gameObject.tag == "Book"){
            // Empty method for now. Fill in code later if you want code to be run when an book leaves a player's hitbox.
        }
    }
    #endregion

    #region Enable/disable player movement methods.
    public static void DisablePlayerMovement(){
        movement.enabled = false; 
        StaticCoroutine.Start(SlowDownPlayer());
    }

    public static IEnumerator SlowDownPlayer(){
        yield return new WaitForSeconds(0.5f);
        // Freeze rb x position.
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
        yield return new WaitForSeconds(0.5f);
        // Unfreeze rb x position.
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    public static void EnablePlayerMovement(){
        // Enable player movement.
        movement.enabled = true;
    }
    #endregion

    #region Health system methods.
    public static void TakeDamage(float damageAmount){
        int numFlashes = 6;
        float timeBetweenFlashes = 0.25f;
        StaticCoroutine.Start(FlashRed(numFlashes, timeBetweenFlashes));
        HealthManager.Instance.ChangeHealth(damageAmount, false);

        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies){
            enemy.GetComponent<Enemy>().DisablePlayerCollisions();
            enemy.GetComponent<Enemy>().Invoke("EnablePlayerCollisions", secWaitAfterCollision);
        }
    }

    public static IEnumerator FlashRed(int numFlashes, float timeBetweenFlashes){
        //int numFlashes = 3;
        for (int i = 0; i < numFlashes; i++){
            playerSprite.color = Color.red;
            yield return new WaitForSeconds(timeBetweenFlashes);
            playerSprite.color = Color.white;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
    }

    public static void Faint(){

    }
    #endregion
}

