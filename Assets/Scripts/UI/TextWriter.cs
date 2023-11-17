using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Cinemachine;

public class TextWriter : MonoBehaviour
{
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI nameText; // This is the name of the person speaking
    public Image dialogueSprite;
    public Sprite playerSprite; // This serves as a reference for what the player sprite is
    public GameObject downArrow;
    public GameObject textBox;
    public static GameObject textBoxStatic;
    public static bool isWritingText = false;
    public static ConversationSO conversation;
    //public static string[] textsToWrite;
    //public static Sprite[] spritesWithText;
    private static int textsToWriteIndex;
    private int characterIndex = 0;
    [SerializeField] public static float timePerCharacter = 0.025f;
    [SerializeField] public static float originalTimePerCharacter = 0.025f;
    private float timer = 0;
    public CinemachineVirtualCamera vcam;
    public static CinemachineVirtualCamera vcamStatic;
    public float convoCamSize; // This is the size of the camera when player is trapped in a conversation
    private static float convoCamSizeStatic;
    private static float originalCamSizeStatic; // This is the original size of the camera.

    void Awake(){
        textBoxStatic = textBox;
        vcamStatic = vcam;
        originalCamSizeStatic = vcam.m_Lens.OrthographicSize;
        convoCamSizeStatic = convoCamSize;
    }

    // The below method is a general method to activate conversations. The "texts" parameter takes on an unlimited amount of texts.
    // Pass in the strings that you want to be triggered one after the other (like a real conversation) into the "texts" parameter.
    // The player will only be able to move from one text to another after pressing the Enter key on keyboard.
    public static void ActivateConversation(ConversationSO newConversation)
    {
        textBoxStatic.SetActive(true);
        isWritingText = true;
        conversation = newConversation;
        //textsToWrite = conversation.texts;
        //spritesWithText = conversation.sprites;
        vcamStatic.m_Lens.OrthographicSize = convoCamSizeStatic;
        FreezeEnemies();
    }

    public static void DeactivateConversation(){
        textBoxStatic.SetActive(false);
        isWritingText = false;
        //textsToWrite = null;
        conversation = null;
        textsToWriteIndex = 0;
        vcamStatic.m_Lens.OrthographicSize = originalCamSizeStatic;
        UnfreezeEnemies();
        Player.EnablePlayerMovement();
        TextWriter.timePerCharacter = TextWriter.originalTimePerCharacter;
        TimeManager.minuteToRealTime = TimeManager.originalMinuteToRealTime;
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
            //dialogueSprite.sprite = spritesWithText[textsToWriteIndex];
            dialogueSprite.sprite = conversation.sprites[textsToWriteIndex];
            if (dialogueSprite.sprite == playerSprite){
                nameText.text = "Player";
            } else {
                nameText.text = conversation.npcName;
            }
            //if (characterIndex < textsToWrite[textsToWriteIndex].Length){
            if (characterIndex < conversation.texts[textsToWriteIndex].Length){
                timer -= Time.deltaTime;
                if (timer <= 0f){
                    timer += timePerCharacter;
                    characterIndex++;
                    //dialogueText.text = textsToWrite[textsToWriteIndex].Substring(0, characterIndex);
                    dialogueText.text = conversation.texts[textsToWriteIndex].Substring(0, characterIndex);
                }
            } else {
                downArrow.SetActive(true);
                if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.Space)){
                    textsToWriteIndex++;
                    characterIndex = 0;
                    timer = 0;
                    downArrow.SetActive(false);
                    //if (textsToWriteIndex == textsToWrite.Length - 1){
                    if (textsToWriteIndex == conversation.texts.Length - 1){
                        DeactivateConversation();
                    }
                }
            }
        }
    }
}
