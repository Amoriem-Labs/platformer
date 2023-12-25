using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class PlayerInventory : MonoBehaviour
{
    public TextMeshProUGUI assignmentText; // This is the text that displays the number of assignments collected.
    public int numItems { get; private set; } // This will store the number of collected assignments.
    public static Action OnItemCollected; // This is the event that will be invoked when an item is collected.
    public GameObject levelCompleteMessage; // This is the message that will be displayed when the level is complete.

    void OnEnable()
    {
        numItems = 0;
        levelCompleteMessage.SetActive(false);
        OnItemCollected += UpdateAssignmentText;
    }

    void OnDisable()
    {
        OnItemCollected -= UpdateAssignmentText;
    }

    public void ItemCollected()
    {
        numItems++;
        OnItemCollected?.Invoke();
        if (numItems == GameManager.Instance.currentLevel.numAssignmentsToComplete)
        {
            StartCoroutine(DisplayLevelCompleteMessage());
        }
    }

    public void UpdateAssignmentText()
    {
        assignmentText.text = numItems.ToString();
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

    [ContextMenu("Test Display Message")]
    public void TestDisplayMessage()
    {
        StartCoroutine(DisplayLevelCompleteMessage());
    }
}

