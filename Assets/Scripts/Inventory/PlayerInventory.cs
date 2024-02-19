using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public InventoryItem[] inventoryItems; // This will store the items that the player has collected.
    public int numAssignments { get; private set; } // This will store the number of collected assignments.
    public int numCoins { get; private set; } // This will store the number of collected coins.
    public static Action OnItemCollected; // This is the event that will be invoked when an item is collected.
    public GameObject levelCompleteMessage; // This is the message that will be displayed when the level is complete.

    void OnEnable()
    {
        numAssignments = 0;
        numCoins = 0;
        levelCompleteMessage.SetActive(false);
        OnItemCollected += UpdateAssignmentText;
        OnItemCollected += UpdateCoinText;
    }

    void OnDisable()
    {
        OnItemCollected -= UpdateAssignmentText;
        OnItemCollected -= UpdateCoinText;
    }

    public void ItemCollected(GameObject item)
    {
        if (item.tag == "Coin")
        {
            numCoins++;
            GameManager.Instance.levelGradingManager.numCoinsCollected++;
        }
        else if (item.tag == "Assignment")
        {
            numAssignments++;
        }
        OnItemCollected?.Invoke();
        if (numAssignments == GameManager.Instance.currentLevel.numAssignmentsToComplete)
        {
            GameManager.Instance.levelCompleted = true;
            StartCoroutine(DisplayLevelCompleteMessage());
        }
    }

    public void UpdateAssignmentText()
    {
        GameManager.Instance.assignmentText.text = $"{numAssignments}/{GameManager.Instance.currentLevel.numAssignmentsToComplete}";
    }

    public void UpdateCoinText(){
        GameManager.Instance.coinText.text = $"{numCoins}";
    }

    IEnumerator DisplayLevelCompleteMessage()
    {
        levelCompleteMessage.SetActive(true);
        // Have the level complete message drop from above the screen.
        for (int i = 0; i < 4; i++){
            levelCompleteMessage.transform.position = new Vector3(levelCompleteMessage.transform.position.x, levelCompleteMessage.transform.position.y - 0.64f, levelCompleteMessage.transform.position.z);
            yield return new WaitForSeconds(0.05f);
        }

        // Have the level complete message bounce up and down 3 times.
        for (int numIter = 0; numIter < 3; numIter++){
            for (int i = 0; i < 10; i++){
                levelCompleteMessage.transform.position = new Vector3(levelCompleteMessage.transform.position.x, levelCompleteMessage.transform.position.y + 0.08f, levelCompleteMessage.transform.position.z);
                yield return new WaitForSeconds(0.05f);
            }
            for (int i = 0; i < 10; i++){
                levelCompleteMessage.transform.position = new Vector3(levelCompleteMessage.transform.position.x, levelCompleteMessage.transform.position.y - 0.08f, levelCompleteMessage.transform.position.z);
                yield return new WaitForSeconds(0.05f);
            }
        }

        // Have the level complete message rise above the screen.
        for (int i = 0; i < 4; i++){
            levelCompleteMessage.transform.position = new Vector3(levelCompleteMessage.transform.position.x, levelCompleteMessage.transform.position.y + 0.64f, levelCompleteMessage.transform.position.z);
            yield return new WaitForSeconds(0.05f);
        }
        levelCompleteMessage.SetActive(false);
    }
}

