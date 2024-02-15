using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System.Linq;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }
    [HideInInspector] public Level currentLevel;
    [HideInInspector] public Level[] levels;
    public Animator animator;
    public bool levelCompleted = false;
    public TextMeshProUGUI assignmentText;
    public TextMeshProUGUI coinText;
    public SleepTimer sleepTimer;
    public SaveData currSaveData;
    private GameObject player;
    public LevelGradingManager levelGradingManager;
    public string levelGradingSceneName;
    public delegate void OnSave();
    public static event OnSave onSave;

    // IMPORTANT NOTE FOR DEVELOPERS: Do NOT call SceneManager.LoadScene in Awake or Start. This will cause the scene to load twice and make Unity get stuck in an infinite loading loop.
    // You have been warned.
    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);

            player = GameObject.FindGameObjectWithTag("Player");
            animator.enabled = false;

            levels = Resources.LoadAll<Level>("Levels/");
            currentLevel = levels[0];
            assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}";
            coinText.text = "0";
        }
    }

    void Update(){
        if (sleepTimer.timeInTimer <= 0f){
            ResetLevel();
        }
    }

    public void DisablePlayer(){
        player.SetActive(false);
    }

    [ContextMenu("RespawnPlayer")]
    public void RespawnPlayer(){
        player.SetActive(true);
        player.transform.position = new Vector3(-9.11f, 0f, 0f);
    }

    public void LoadLevelGradingScreen(){
        animator.enabled = false;
        animator.enabled = true;
        DisablePlayer();
        SceneManager.LoadScene(levelGradingSceneName);
    }

    [ContextMenu("LoadNextLevel")]
    public void LoadNextLevel(){
        animator.enabled = false;
        animator.enabled = true;
        levelCompleted = false;
        // Call below functions only when animation is completed
        currentLevel = levels[currentLevel.levelID + 1];
        levelGradingManager.ResetNumCoinsCollected();
        SceneManager.LoadScene(currentLevel.sceneName);
        Invoke("RespawnPlayer", 1f);
    }

    public void LoadLevel(int levelID){
        animator.enabled = false;
        animator.enabled = true;
        levelCompleted = false;
        // Call below functions only when animation is completed
        currentLevel = levels[levelID];
        sleepTimer.maxTime = currentLevel.maxTime;
        assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}";
        levelGradingManager.ResetNumCoinsCollected();
        SceneManager.LoadScene(currentLevel.sceneName);
        player.transform.position = currentLevel.playerSpawnPoint;
    }

    public void ResetLevel(){
        animator.enabled = false;
        animator.enabled = true;
        levelCompleted = false;
        levelGradingManager.ResetNumCoinsCollected();
        // Call below functions only when animation is completed
        SceneManager.LoadScene(currentLevel.sceneName);
        player.transform.position = currentLevel.playerSpawnPoint;
    }

    #region Save methods.
    // Saves begin from index 0.
    public void LoadSave(int saveIndex)
    {
        // Delete current in-game dynamic data and replace it with save file data
        currSaveData = null;

        SaveUtil.ReadFile(ref currSaveData, saveIndex); // If successfully able to read in file, currSaveData should no longer be null. If unsuccessful, currSaveData remains null

        // Here is where I load in the save data.
        if(currSaveData != null)
        {
            // Read in the level from the save data
            currentLevel = levels[currSaveData.levelID];
            LoadLevel(currentLevel.levelID);

            // Method to instantiate player and set player position should be within LoadLevel
        }
        else
        {
            // If no saves on file, the below lines of code are called
            // Load in the scene
            assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}";
            coinText.text = "0";
            LoadLevel(0);
        }
    }

    public void WriteToSave(int saveIndex)
    {
        SaveUtil.WriteFile(ref currSaveData, saveIndex);
    }

    // This function is subject to change and finalization as number of save variables increase!
    public void CreateNewSave(int saveIndex)
    {
        onSave?.Invoke();
        SaveData newSave = new SaveData();

        // deal with levels
        newSave.levelID = currentLevel.levelID;

        // write the current save data to the saveIndex save
        SaveUtil.WriteFile(ref newSave, saveIndex);
    }

    // Test function to create a new save file
    [ContextMenu("Create New Save")]
    public void TestCreatingNewSave()
    {
        CreateNewSave(0);
    }
    #endregion
}
