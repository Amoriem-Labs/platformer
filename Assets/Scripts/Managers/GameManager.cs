using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }
    public Level currentLevel;
    public Level[] levels;
    public Animator animator;
    public bool levelCompleted = false;
    public TextMeshProUGUI assignmentText;
    public TextMeshProUGUI coinText;

    // IMPORTANT NOTE FOR DEVELOPERS: Do NOT call SceneManager.LoadScene in Awake or Start. This will cause the scene to load twice and make Unity get stuck in an infinite loading loop.
    // You have been warned.
    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);

            animator.enabled = false;

            levels = Resources.LoadAll<Level>("Levels/");
            currentLevel = levels[0];
            assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}";
            coinText.text = "0";
        }
    }

    [ContextMenu("LoadNextLevel")]
    public void LoadNextLevel(){
        animator.enabled = false;
        animator.enabled = true;
        levelCompleted = false;
        // Call below functions only when animation is completed
        currentLevel = levels[currentLevel.levelID + 1];
        SceneManager.LoadScene(currentLevel.sceneName);
    }

    [ContextMenu("TestFadeOut")]
    public void TestFadeOut(){
        animator.enabled = true;
    }
}
