using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject textBox; // This is a reference to the text box that will be triggered when the opp runs into the player.
    public Rigidbody2D rb;


    void Start(){
        textBox.SetActive(false);
    }

    // When an opp runs into the player, trigger a conversation that prevents the player from moving until the conversation is over.
    void OnCollisionEnter2D(Collision2D collision){
        int layer = LayerMask.NameToLayer("Opp");
        if (collision.gameObject.layer == layer){
            // TODO: make better voice dialogues
            string[] texts = new string[]{"Heyyy, you're looking fine today!", "Thanks, I appreciate it.", "Ooh, what's that accent from?", "Brooklyn.", "I have to say, I really like your fit.", "Thanks.", "We should grab coffee sometime, I would love to learn what else you got in your wardrobe. What's your number?", "Oh, I'm sorry, I gotta go to class right now, I can't talk. Bye!", "What?"};
            TextWriter.activateConversation(texts);

            // TODO: WRITE CODE TO PAUSE PLAYER MOVEMENT
        }
    }
}
