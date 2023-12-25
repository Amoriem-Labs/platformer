using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
	public static GameManager Instance { get { return _instance; } }
    public Level currentLevel;
    public Level[] levels;

    // IMPORTANT NOTE FOR DEVELOPERS: Do NOT call SceneManager.LoadScene in Awake or Start. This will cause the scene to load twice and make Unity get stuck in an infinite loading loop.
    // You have been warned.
    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);

            levels = Resources.LoadAll<Level>("Levels/");

            currentLevel = levels[0];
        }
    }

    public void LoadNextLevel(){
        currentLevel = levels[currentLevel.levelID + 1];
        SceneManager.LoadScene(currentLevel.sceneName);
    }
}
