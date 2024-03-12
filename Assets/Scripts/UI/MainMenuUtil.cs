using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUtil : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu_VL";

    public void QuitGame(){
        Application.Quit();
    }

    public void ReturnToMainMenu(){
        SceneManager.LoadScene(mainMenuSceneName);
    }
}
