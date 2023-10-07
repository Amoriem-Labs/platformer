using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject textBox; // This is a reference to the text box that will be triggered when the opp runs into the player.
    public TriggerResponse oppTriggerResponse;

    void Start(){
        textBox.SetActive(false);
        oppTriggerResponse.onTriggerEnter2D = OnOppDetectorTriggerEnter2D;
        oppTriggerResponse.onTriggerExit2D = OnOppDetectorTriggerExit2D;
    }

    #region Collider methods.
    // When an opp runs into the player, trigger a conversation that prevents the player from moving until the conversation is over.
    private void OnOppDetectorTriggerEnter2D(Collider2D collision)
    {
        int layer = LayerMask.NameToLayer("Opp");
        if (collision.gameObject.layer == layer)
        {
            Opp opp = collision.gameObject.GetComponent<Opp>();
            if (opp.isOppTriggerOn){
                // TODO: make better voice dialogues
                string[] texts = new string[]{"Heyyy, you're looking fine today!", "Thanks, I appreciate it.", "Ooh, what's that accent from?", "Brooklyn.", "I have to say, I really like your fit.", "Thanks.", "We should grab coffee sometime, I would love to learn what else you got in your wardrobe. What's your number?", "Oh, I'm sorry, I gotta go to class right now, I can't talk. Bye!", "What?"};
                TextWriter.activateConversation(texts);

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
