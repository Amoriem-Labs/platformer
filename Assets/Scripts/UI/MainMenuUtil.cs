using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuUtil : MonoBehaviour
{
    public string mainMenuSceneName = "MainMenu_VL";

    public void StartGame(){
        PersistentObject.Instance.SetActive();
        GameManager.Instance.LoadSave(0);
    }

    public void QuitGame(){
        Application.Quit();
    }

    public void ReturnToMainMenu(){
        SceneManager.LoadScene(mainMenuSceneName);
    }

    public void DeleteSave(){
        SaveUtil.DeleteSaveFile(0);
    }
}
