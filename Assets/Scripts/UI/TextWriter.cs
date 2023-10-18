using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TextWriter : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public Image dialogueSprite;
    public GameObject downArrow;
    public GameObject textBox;
    public static GameObject textBoxStatic;
    public static bool isWritingText = false;
    public static string[] textsToWrite;
    public static Sprite[] spritesWithText;
    private static int textsToWriteIndex;
    private int characterIndex = 0;
    public float timePerCharacter;
    private float timer = 0;

    void Awake(){
        textBoxStatic = textBox;
    }

    // The below method is a general method to activate conversations. The "texts" parameter takes on an unlimited amount of texts.
    // Pass in the strings that you want to be triggered one after the other (like a real conversation) into the "texts" parameter.
    // The player will only be able to move from one text to another after pressing the Enter key on keyboard.
    public static void ActivateConversation(ConversationSO conversation)
    {
        textBoxStatic.SetActive(true);
        isWritingText = true;
        textsToWrite = conversation.texts;
        spritesWithText = conversation.sprites;
        FreezeEnemies();
        // TODO: add in a sprite to the text box so that player knows who is talking which text
    }

    public static void DeactivateConversation(){
        textBoxStatic.SetActive(false);
        isWritingText = false;
        textsToWrite = null;
        textsToWriteIndex = 0;
        UnfreezeEnemies();
    }

    // Freezes all enemies on screen
    public static void FreezeEnemies(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies){
            enemy.GetComponent<Enemy>().isFrozen = true;
        }
    }

    // Unfreezes all enemies on screen
    public static void UnfreezeEnemies(){
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("Enemy");
        foreach (var enemy in enemies){
            enemy.GetComponent<Enemy>().isFrozen = false;
        }
    }

    void Update()
    {
        if (isWritingText){
            dialogueSprite.sprite = spritesWithText[textsToWriteIndex];
        
            if (characterIndex < textsToWrite[textsToWriteIndex].Length){
                timer -= Time.deltaTime;
                if (timer <= 0f){
                    timer += timePerCharacter;
                    characterIndex++;
                    dialogueText.text = textsToWrite[textsToWriteIndex].Substring(0, characterIndex);
                }
            } else {
                downArrow.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
                    textsToWriteIndex++;
                    characterIndex = 0;
                    timer = 0;
                    downArrow.SetActive(false);
                    if (textsToWriteIndex == textsToWrite.Length - 1){
                        DeactivateConversation();
                    }
                }
            }
        }
    }
}
