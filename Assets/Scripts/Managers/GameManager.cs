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
    public Animator fadeAnim;
    public bool levelCompleted = false;
    public TextMeshProUGUI assignmentText;
    public TextMeshProUGUI coinText;
    public SleepTimer sleepTimer;
    public SaveData currSaveData;
    [HideInInspector] public GameObject player;
    private PlayerInventory playerInventory;
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
            playerInventory = player.GetComponent<PlayerInventory>();
            fadeAnim.enabled = false;

            levels = Resources.LoadAll<Level>("Levels/");
            currentLevel = levels[0];
            assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}";
            coinText.text = "0";
        }
    }

    void Update(){
        if (sleepTimer.timeInTimer <= 0f){
            ResetLevel(true);
        }
    }

    public void LoadLevelGradingScreen(){
        fadeAnim.enabled = false;
        fadeAnim.enabled = true;
        SceneManager.LoadScene(levelGradingSceneName);
    }

    [ContextMenu("LoadNextLevel")]
    // Function purely for debugging purposes, only ever called from Inspector
    public void LoadNextLevel(){
        LoadLevel(currentLevel.levelID + 1);
    }

    public void LoadLevel(int levelID){
        currentLevel = levels[levelID];
        ResetLevel(false);
    }

    // Function to reset the level, and has two uses: if the player died or if the player completed the level
    public void ResetLevel(bool playerDied){
        AudioManager.Instance.StopMusic(); // Stop music
        fadeAnim.enabled = false; // Reset fade out/in animation
        fadeAnim.enabled = true;
        levelCompleted = false; // Reset level completion status
        LevelGradingManager.Instance.ResetNumCoinsCollected(); // Reset number of coins collected used in scoring system
        if (playerDied){
            playerInventory.numCoins -= LevelGradingManager.Instance.numCoinsCollected; // Remove coins collected from player inventory if resetting level because player died, otherwise keep number of coins in inventory because player has moved onto another level
            coinText.text = $"{playerInventory.numCoins}";
        } else {
            sleepTimer.maxTime = currentLevel.maxTime; // Changing sleep timer max time for new level for case #2: when player has completed the previous level
        }
        AudioManager.Instance.StartMusic(); // Restart music
        sleepTimer.ResetTimer(); // Reset sleep timer
        assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}"; // Reset number of assignments completed
        // Call below functions only when fade out/in animation is completed
        SceneManager.LoadScene(currentLevel.sceneName); // Load in the scene
        player.transform.position = currentLevel.playerSpawnPoint; // Reset player position
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
