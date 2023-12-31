using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[CreateAssetMenu(menuName = "ConversationSO")]
public class ConversationSO : ScriptableObject
{
    public string npcName;
    public string[] texts; // This is the conversation that the opp will trigger upon colliding with player.
    public Sprite[] sprites; // This is the list of sprites that go along with each text in the conversation. The sprites correspond to who is speaking which line. Make sure the length of the sprite array is the same as the length of the text array.
}
