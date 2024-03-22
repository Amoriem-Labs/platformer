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

    [Header("Level-related Variables")]
    public Level currentLevel;
    public Level[] levels;
    public Animator fadeAnim;
    public bool levelCompleted = false;
    
    [Header("Pause Menu Related Variables")]
    public GameObject settingsPopup;
    public GameObject audioPopup;
    public GameObject mainMenuPopup;
    public bool isGamePaused;

    [Header("Save-related Variables")]
    public SaveData currSaveData;
    public delegate void OnSave();
    public static event OnSave onSave;
    public Slider musicVolumeSlider;
    public Slider sfxVolumeSlider;

    [Header("Miscellaneous Variables")]
    [HideInInspector] public GameObject player;
    public string levelGradingSceneName;
    public CameraController cameraController;

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

            levels = Resources.LoadAll<Level>("Levels");
            currentLevel = levels[0];
            isGamePaused = false;
        }
    }

    public void StartGame(){
        currentLevel = levels[0];
        AudioManager.Instance.GetSoundtrack(currentLevel.nameOfSoundtrack).time = AudioManager.Instance.soundtrackBeginTime;
        AudioManager.Instance.GetSoundtrack(currentLevel.nameOfSoundtrack).Play();
        SceneManager.LoadScene(currentLevel.sceneName); // Load in the scene
        player.transform.position = currentLevel.playerSpawnPoint; // Reset player position
    }

    public void PauseGame(){
        isGamePaused = true;
        Time.timeScale = 0f;
        settingsPopup.SetActive(true);
        AudioManager.Instance.PauseSound();
        Animator[] animators = (Animator[])GameObject.FindObjectsOfType(typeof(Animator));
        foreach (Animator anim in animators){
            anim.speed = 0;
        }
    }

    public void ResumeGame(){
        isGamePaused = false;
        Time.timeScale = 1.0f;
        settingsPopup.SetActive(false);
        AudioManager.Instance.ResumeSound();
        Animator[] animators = (Animator[])GameObject.FindObjectsOfType(typeof(Animator));
        foreach (Animator anim in animators){
            anim.speed = 1;
        }
    }

    #region Level Related Functions
    public void LoadLevelGradingScreen(){
        fadeAnim.enabled = false;
        fadeAnim.enabled = true;
        WriteToSave(0);
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
        AudioManager.Instance.StopSound(); // Stop music
        fadeAnim.enabled = false; // Reset fade out/in animation
        fadeAnim.enabled = true;
        levelCompleted = false; // Reset level completion status
        if (playerDied){
            CoinManager.Instance.numCoinsPlayerHas -= CoinManager.Instance.numCoinsCollectedInLevel; // Remove coins collected from player inventory if resetting level because player died, otherwise keep number of coins in inventory because player has moved onto another level
        } else {
            SleepManager.Instance.maxTime = currentLevel.maxTime; // Changing sleep timer max time for new level for case #2: when player has completed the previous level
        }
        AudioManager.Instance.GetSoundtrack(currentLevel.nameOfSoundtrack).Play(); // Restart music
        SleepManager.Instance.ResetTimer(); // Reset sleep timer
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
            Debug.Log("Save file being loaded...");
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
            Debug.Log("Save file does not exist, creating new game...");
            // If no saves on file, the below lines of code are called
            // Load in the scene
            StartGame();
            AssignmentManager.Instance.assignmentText.text = $"0/{currentLevel.numAssignmentsToComplete}";
            CoinManager.Instance.coinText.text = "0";
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

    // Test function to create a new save file
    [ContextMenu("Create New Save")]
    public void TestCreatingNewSave()
    {
        CreateNewSave(0);
    }
    #endregion
}
