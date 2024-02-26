using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AssignmentManager : MonoBehaviour
{
    private static AssignmentManager _instance;
	public static AssignmentManager Instance { get { return _instance; } }
    public int numAssignments; // This will store the number of collected assignments.
    public GameObject levelCompleteMessage; // This is the message that will be displayed when the level is complete.
    public TextMeshProUGUI assignmentText; // This is the text that will be displayed in the UI to show the number of assignments the player has.

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);

            numAssignments = 0;
            levelCompleteMessage.SetActive(false);
        }
    }

    public void AddAssignment(){
        numAssignments++;
        UpdateAssignmentText();
        if (numAssignments == GameManager.Instance.currentLevel.numAssignmentsToComplete)
        {
            GameManager.Instance.levelCompleted = true;
            StartCoroutine(DisplayLevelCompleteMessage());
        }
    }

    public void ResetAssignmentsToZero(){
        numAssignments = 0;
    }

    public void UpdateAssignmentText()
    {
        assignmentText.text = $"{numAssignments}/{GameManager.Instance.currentLevel.numAssignmentsToComplete}";
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
