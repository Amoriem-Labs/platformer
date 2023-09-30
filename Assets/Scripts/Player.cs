using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject textBox; // This is a reference to the text box that will be triggered when the opp runs into the player.

    void Start(){
        textBox.SetActive(false);
    }

    // When an opp runs into the player, trigger a conversation that prevents the player from moving until the conversation is over.
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Opp");
        if (collision.gameObject.layer == layer){
            string[] texts = new string[]{"filler text 1", "filler text 2", "filler text 3", "filler text 4"};
            activateConversation(texts);

            // TODO: WRITE CODE TO PAUSE PLAYER MOVEMENT
        }
    }
    
    // The below method is a general method to activate conversations. The "texts" parameter takes on an unlimited amount of texts.
    // Pass in the strings that you want to be triggered one after the other (like a real conversation) into the "texts" parameter.
    // The player will only be able to move from one text to another after pressing the Enter key on keyboard.
    void activateConversation(params string[] texts)
    {
        textBox.SetActive(true);
        // TODO: add in text writing effect to code
        // TODO: add in a sprite to the text box so that player knows who is talking which text
        textBox.SetActive(false);
    }
}
