using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class LevelScoreScreen : MonoBehaviour
{
    public GameObject gradeA;
    public GameObject gradeB;
    public GameObject gradeC;
    public GameObject gradeD;
    public GameObject gradeF;
    private GameObject gradeImage;
    public float timeBetweenValueChanges = 0.1f;
    public GameObject timeText;
    public TextMeshProUGUI timeSpentText;
    public GameObject coinText;
    public TextMeshProUGUI coinsCollectedText;
    public GameObject nextLevelButton;
    public int countNumbersUpAnimationDuration;
    public int cameraShakeIntensity;
    public CinemachineVirtualCamera vcam;
    public Transform scoreboard;

    [ContextMenu("StartAnimation")]
    void Start(){
        vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        vcam.LookAt = scoreboard;
        vcam.Follow = scoreboard;
        ResetScreen();
        string grade = GameManager.Instance.levelScoringManager.GetGrade();
        switch (grade){
            case "A":
                gradeImage = gradeA;
                break;
            case "B":
                gradeImage = gradeB;
                break;
            case "C":
                gradeImage = gradeC;
                break;
            case "D":
                gradeImage = gradeD;
                break;
            case "F":
                gradeImage = gradeF;
                break;
        }
        StartGradeShakeCameraAnimation();
    }

    public void StartGradeShakeCameraAnimation(){
        gradeImage.SetActive(true);
        Invoke("StartShakeCameraAnimation", 0.525f); // Wait ~half a second before starting the camera shake animation
        Invoke("StartTimeShakeCameraAnimation", 0.525f+0.01f); // Wait ~half a second after camera shake animation before starting next animation
    }

    public void StartTimeShakeCameraAnimation(){
        timeText.SetActive(true);
        Invoke("StartShakeCameraAnimation", 0.525f); // Wait a few seconds before starting the camera shake 
        Invoke("StartCoinShakeCameraAnimation", 0.525f+0.01f); 
    }

    public void StartCoinShakeCameraAnimation(){
        coinText.SetActive(true);
        Invoke("StartShakeCameraAnimation", 0.525f); // Wait a few seconds before starting the camera shake
        Invoke("StartCountingUpAnimation", 0.525f+0.01f); 
    }

    public void StartShakeCameraAnimation(){
        StartCoroutine(ShakeCamera());
    }

    IEnumerator ShakeCamera(){
        CinemachineBasicMultiChannelPerlin multiChannelPerlin = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        multiChannelPerlin.m_AmplitudeGain = cameraShakeIntensity;
        yield return new WaitForSeconds(0.01f);
        multiChannelPerlin.m_AmplitudeGain = 0;
        StopCoroutine(ShakeCamera());
    }

    public void StartCountingUpAnimation(){
        StartCoroutine(ChangeValue(timeSpentText, GameManager.Instance.levelScoringManager.sleepTimer.timeSpent, timeBetweenValueChanges));
        //StartCoroutine(ChangeValue(timeSpentText, 100, timeBetweenValueChanges));
        StartCoroutine(ChangeValue(coinsCollectedText, GameManager.Instance.levelScoringManager.numCoinsCollected, timeBetweenValueChanges));
        //StartCoroutine(ChangeValue(coinsCollectedText, 10, timeBetweenValueChanges));
    }
    
    IEnumerator ChangeValue(TextMeshProUGUI text, float finalValue, float timeBetweenValueChanges){
        for (int val = 0; val <= finalValue; val++){
            text.text = val.ToString();
            yield return new WaitForSeconds(timeBetweenValueChanges);
        }
        nextLevelButton.SetActive(true);
    }

    public void LoadNextLevel(){
        vcam.LookAt = null;
        vcam.Follow = null;
        GameManager.Instance.LoadLevel(GameManager.Instance.currentLevel.levelID + 1);
    }
    
    [ContextMenu("ResetValue")]
    public void ResetScreen(){
        gradeA.SetActive(false);
        gradeB.SetActive(false);
        gradeC.SetActive(false);
        gradeD.SetActive(false);
        gradeF.SetActive(false);
        timeText.SetActive(false);
        coinText.SetActive(false);
        nextLevelButton.SetActive(false);
        timeSpentText.text = "";
        coinsCollectedText.text = "";
    }

    [ContextMenu("StopCoroutine")]
    public void StopCoroutine(){
        StopAllCoroutines();
    }
}
