using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject textBox; // This is a reference to the text box that will be triggered when the opp runs into the player.
    public TriggerResponse oppTriggerResponse; // This is a TriggerResponse script that creates a custom collider between only the player and opps.
    public float numSecondsFreeze; // This is the number of seconds to freeze an enemy.

    void Start(){
        textBox.SetActive(false);
        oppTriggerResponse.onTriggerEnter2D = OnOppDetectorTriggerEnter2D;
        oppTriggerResponse.onTriggerExit2D = OnOppDetectorTriggerExit2D;
    }

    #region Freezing enemies.
    // Freezes all enemies on screen for numSecondsFreeze seconds. Remove this function in final production later. This function is purely for testing purposes.
    [ContextMenu("Freeze Enemies")]
    public void TestFreezeEnemies(){
        FreezeEnemies(numSecondsFreeze);
    }

    // Freezes all enemies on screen for X seconds. Re
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
    private void OnOppDetectorTriggerEnter2D(Collider2D collision)
    {
        int layer = LayerMask.NameToLayer("Opp");
        if (collision.gameObject.layer == layer)
        {
            Opp opp = collision.gameObject.GetComponent<Opp>();
            if (opp.isThisOppTriggerOn){
                TextWriter.ActivateConversation(opp.conversation);

                // TODO: WRITE CODE TO PAUSE PLAYER MOVEMENT
            }
        }
    }

    private void OnOppDetectorTriggerExit2D(Collider2D collision)
    {
        int layer = LayerMask.NameToLayer("Opp");
        if (collision.gameObject.layer == layer)
        {
            // Empty method for now. Fill in code later if you want code to be run when an opp leaves a player's hitbox.
        }
    }
    #endregion
}
