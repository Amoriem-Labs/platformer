using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuChecker : MonoBehaviour
{
    void Update(){
        if (!GameManager.Instance.isGamePaused){
            if (GameManager.Instance.sleepTimer.timeInTimer <= 0f){
                GameManager.Instance.ResetLevel(true);
            }
        }
        // When pressing Escape in Return to Main Menu and Audio Settings popups, always return to Settings Popup
        if (Input.GetKeyDown(KeyCode.Escape)){
            if (!GameManager.Instance.isGamePaused){
                GameManager.Instance.PauseGame();
            } else {
                if (GameManager.Instance.settingsPopup.activeSelf){
                    GameManager.Instance.ResumeGame();
                } else {
                    GameManager.Instance.settingsPopup.SetActive(true);
                    GameManager.Instance.audioPopup.SetActive(false);
                    GameManager.Instance.mainMenuPopup.SetActive(false);
                }
            }
        }
    }
}
