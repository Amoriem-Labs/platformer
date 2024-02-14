using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using MoreMountains.Tools;
using MoreMountains.Feedbacks;
using MoreMountains.CorgiEngine;

public class Player : MonoBehaviour
{
    public GameObject textBox; // This is a reference to the text box that will be triggered when the opp runs into the player.
    public static Rigidbody2D rb;
    public TriggerResponse oppTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the player and opps.
    public float numSecondsFreeze; // This is the number of seconds to freeze an enemy.
    public float numSecondsShield; // This is how long the player's shield is activated in seconds.
    public SpriteRenderer shield; // This is the shield sprite.
    public static WASDMovement movement; // Script for player movement
    public float respawnOffset; // The offset from last grounded position when respawning player.
    private static SpriteRenderer playerSprite; // This is the player's sprite renderer.
    public static float secWaitAfterCollision = 2.5f; // This is the number of seconds to wait after a collision before re-enabling player collisions.
    public static Vector3 lastGroundedPosition; // This is the last position the player was grounded at.
    public static bool isOnFire = false; // This is whether or not the player is on fire.
    public float fireDamageAmount = 0.5f; // This is the amount of damage the fire does to the player every cycle.
    private float timeUntilNextFireDamage; // This is the time until the player takes fire damage again.
    public float timeBetweenFireDamage; // This is the time between fire damage.
    public GameObject onFireAnimator; // This is the GameObject for the on fire animation when player is on fire.
    public static bool isPoisoned = false; // This is whether or not the player is on fire.
    public float poisonDamageAmount = 0.5f; // This is the amount of damage the poison does to the player every cycle.
    private float timeUntilNextPoisonDamage; // This is the time until the player takes poison damage again.
    public float timeBetweenPoisonDamage; // This is the time between poison dmamage.
    public GameObject poisonedAnimator; // This is the GameObject for the poisoned animation when player is poisoned.
    public LayerMask groundLayer; // This is the ground layer.
    public LayerMask platformLayer; // This is the platform layer.
    public Transform groundCheck; // This is the ground check object.
    public float groundCheckRadius = 0.2f; // This is the radius of the ground check.

    void Start(){
        textBox.SetActive(false);
        rb = GetComponent<Rigidbody2D>();
        movement = GetComponent<WASDMovement>();
        oppTriggerResponse.onTriggerEnter2D = OnOppDetectorTriggerEnter2D;
        oppTriggerResponse.onTriggerExit2D = OnOppDetectorTriggerExit2D;
        shield.enabled = false;
        onFireAnimator.SetActive(false);
        poisonedAnimator.SetActive(false);
        playerSprite = GetComponent<SpriteRenderer>();
    }

    void Update(){
        if (Input.GetKeyDown(KeyCode.E) && !TextWriter.isWritingText){
            ActivateShield(numSecondsShield);
        }

        bool isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer) || Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, platformLayer);
        if (isGrounded && rb.velocity.y == 0){
            if (rb.velocity.x > 0) {lastGroundedPosition = transform.position - new Vector3(respawnOffset, 0, 0); }
            if (rb.velocity.x < 0) {lastGroundedPosition = transform.position + new Vector3(respawnOffset, 0, 0); }
        }

        if (isOnFire){
            timeUntilNextFireDamage += Time.deltaTime;
            if (timeUntilNextFireDamage >= timeBetweenFireDamage){
                TakeDamage(fireDamageAmount);
                timeUntilNextFireDamage = 0;
            }
        } else {
            timeUntilNextFireDamage = 0;
        }
        
        if (isPoisoned){
            timeUntilNextPoisonDamage += Time.deltaTime;
            if (timeUntilNextPoisonDamage >= timeBetweenPoisonDamage){
                TakeDamage(poisonDamageAmount);
                timeUntilNextPoisonDamage = 0;
            }
        } else {
            timeUntilNextPoisonDamage = 0;
        }
    }

    #region Shield functions.
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

    #region Set Player on Fire
    public void SetPlayerOnFire(){
        onFireAnimator.SetActive(true);
        isOnFire = true;
    }
    
    public void ExtinguishPlayerFire(){
        onFireAnimator.SetActive(false);
        isOnFire = false;
    }
    #endregion

    #region Poison Player
    public void Poison(){
        poisonedAnimator.SetActive(true);
        isPoisoned = true;
    }
    
    public void CurePoison(){
        poisonedAnimator.SetActive(false);
        isPoisoned = false;
    }
    #endregion

    #region Collider methods.
    // When an opp runs into the player, trigger a conversation that prevents the player from moving until the conversation is over.
    private void OnOppDetectorTriggerEnter2D(Collider2D collider)
    {
        int layer = LayerMask.NameToLayer("Opp");
        if (collider.gameObject.layer == layer)
        {
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies){
                enemy.GetComponent<Enemy>().DisablePlayerCollisions();
            }
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
            GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
            foreach (var enemy in enemies){
                enemy.GetComponent<Enemy>().EnablePlayerCollisions();
            }
        }
    }

    // If player enters next level trigger, load next level.
    private void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.name == "NextLevelTrigger" && GameManager.Instance.levelCompleted)
        {
            GameManager.Instance.LoadLevel(GameManager.Instance.currentLevel.levelID + 1);
            transform.position = new Vector3(-9.11f, -3.74f, 0f);
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
        // will reactivate function once I figure out how to combine multiple sprites into one
        for (int i = 0; i < numFlashes; i++){
            //playerSprite.color = Color.red;
            yield return new WaitForSeconds(timeBetweenFlashes);
            //playerSprite.color = Color.white;
            yield return new WaitForSeconds(timeBetweenFlashes);
        }
    }

    public static void Faint(){

    }
    #endregion
}

