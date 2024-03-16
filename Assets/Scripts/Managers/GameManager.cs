using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
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
    public SleepTimer sleepTimer;
    public SaveData currSaveData;
    [HideInInspector] public GameObject player;
    public string levelGradingSceneName;
    public CameraController cameraController;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;
    public GameObject settingsPopup;
    public bool isGamePaused;
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
            fadeAnim.enabled = false;

            levels = Resources.LoadAll<Level>("Levels/");
            currentLevel = levels[0];
            isGamePaused = false;
        }
    }

    void Update(){
        if (!isGamePaused){
            if (sleepTimer.timeInTimer <= 0f){
                ResetLevel(true);
            }
        }
        // Somehow get button press instead of button taps
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != levelGradingSceneName){
            if (!settingsPopup.activeSelf){
                PauseGame();
            } else {
                ResumeGame();
            }
        }
    }

    public void PauseGame(){
        isGamePaused = true;
        Time.timeScale = 0f;
        settingsPopup.SetActive(true);
        AudioManager.Instance.PauseMusic();
        Animator[] animators = (Animator[])GameObject.FindObjectsOfType(typeof(Animator));
        foreach (Animator anim in animators){
            anim.speed = 0;
        }
    }

    public void ResumeGame(){
        isGamePaused = false;
        Time.timeScale = 1.0f;
        settingsPopup.SetActive(false);
        AudioManager.Instance.StartMusic();
        Animator[] animators = (Animator[])GameObject.FindObjectsOfType(typeof(Animator));
        foreach (Animator anim in animators){
            anim.speed = 1;
        }
    }

    #region Level Related Functions
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
        if (playerDied){
            CoinManager.Instance.numCoinsPlayerHas -= CoinManager.Instance.numCoinsCollectedInLevel; // Remove coins collected from player inventory if resetting level because player died, otherwise keep number of coins in inventory because player has moved onto another level
        } else {
            sleepTimer.maxTime = currentLevel.maxTime; // Changing sleep timer max time for new level for case #2: when player has completed the previous level
        }
        AudioManager.Instance.StartMusic(); // Restart music
        sleepTimer.ResetTimer(); // Reset sleep timer
        CoinManager.Instance.ResetNumCoinsCollectedInLevel(); // Reset number of coins collected used in scoring system
        CoinManager.Instance.UpdateCoinText(); // Update coin text
        AssignmentManager.Instance.ResetAssignmentsToZero(); // Reset number of assignments completed
        AssignmentManager.Instance.UpdateAssignmentText(); // Update assignment text
        // Call below functions only when fade out/in animation is completed
        SceneManager.LoadScene(currentLevel.sceneName); // Load in the scene
        player.transform.position = currentLevel.playerSpawnPoint; // Reset player position
    }
    #endregion

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
            musicVolumeSlider.value = currSaveData.musicVolume;
            sfxVolumeSlider.value = currSaveData.sfxVolume;
            CoinManager.Instance.numCoinsPlayerHas = currSaveData.numCoins;
            CoinManager.Instance.UpdateCoinText();
            LoadLevel(currentLevel.levelID);

            // Method to instantiate player and set player position should be within LoadLevel
        }
        else
        {
            // If no saves on file, the below lines of code are called
            // Load in the scene
            AssignmentManager.Instance.assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}";
            CoinManager.Instance.coinText.text = "0";
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
        newSave.musicVolume = musicVolumeSlider.value;
        newSave.sfxVolume = sfxVolumeSlider.value;

        // write the current save data to the saveIndex save
        SaveUtil.WriteFile(ref newSave, saveIndex);
    }

    public void DeleteSave(){
        // Delete save file
    }

    // Test function to create a new save file
    [ContextMenu("Create New Save")]
    public void TestCreatingNewSave()
    {
        CreateNewSave(0);
    }
    #endregion
}
