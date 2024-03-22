using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using Cinemachine;

public class LevelGradeScreen : MonoBehaviour
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
    private GameObject UICanvas;
    [Tooltip("The minimum score required to get an A")]
    public int A_threshold; 
    [Tooltip("The minimum score required to get an B")]
    public int B_threshold;
    [Tooltip("The minimum score required to get an C")]
    public int C_threshold;
    [Tooltip("The minimum score required to get an D")]
    public int D_threshold;
    [Tooltip("The minimum score required to get an F")]
    public int F_threshold;

    [Tooltip("The maximum number of points a player can get for completing a level in 0 seconds. Player get a percentage of these points linearly proportional to 1 - percentage of time spent relative to sleep timer max time in that level.")]
    public int maxPointsForTime;
    [Tooltip("A weight for how much the time spent on a level should impact the score. Should be between 0 and 1.")]
    public float timeWeight;

    [Tooltip("The maximum number of points a player can get for collecting all the coins in a level. Player get a percentage of these points based off of how many coins they collected in that level.")]
    public int maxPointsForCoins;
    [Tooltip("A weight for how much collecting coins in a level should impact the score. Should be between 0 and 1.")]
    public float coinWeight;
    public float score;

    [ContextMenu("StartAnimation")]
    void Start(){
        if (timeWeight > 1){
            timeWeight = 1;
        }
        if (timeWeight < 0){
            timeWeight = 0;
        }
        if (coinWeight > 1){
            coinWeight = 1;
        }
        if (coinWeight < 0){
            coinWeight = 0;
        }
        vcam = GameObject.Find("Virtual Camera").GetComponent<CinemachineVirtualCamera>();
        vcam.LookAt = scoreboard;
        vcam.Follow = scoreboard;
        UICanvas = GameObject.Find("UICanvas");
        UICanvas.SetActive(false);
        ResetScreen();
        string grade = GetGrade();
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
        Invoke("StartGradeShakeCameraAnimation", 1.5f);
    }

    [ContextMenu("CalculateScore")]
    public float CalculateScore()
    {
        float time_score = (1 - SleepManager.Instance.timeSpent / SleepManager.Instance.maxTime) * maxPointsForTime * timeWeight;
        if (time_score < 0){
            time_score = 0;
        }
        float coin_score = (CoinManager.Instance.numCoinsCollectedInLevel / CoinManager.Instance.numCoinsInLevel) * maxPointsForCoins * coinWeight;
        score = time_score + coin_score;
        return score;
    }

    public string GetGrade()
    {
        CalculateScore();
        if (score >= A_threshold)
        {
            return "A";
        }
        else if (score >= B_threshold)
        {
            return "B";
        }
        else if (score >= C_threshold)
        {
            return "C";
        }
        else if (score >= D_threshold)
        {
            return "D";
        }
        else
        {
            return "F";
        }
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
        StartCoroutine(ChangeValue(timeSpentText, SleepManager.Instance.timeSpent, timeBetweenValueChanges));
        StartCoroutine(ChangeValue(coinsCollectedText, CoinManager.Instance.numCoinsCollectedInLevel, timeBetweenValueChanges));
    }
    
    IEnumerator ChangeValue(TextMeshProUGUI text, float finalValue, float timeBetweenValueChanges){
        for (int val = 0; val <= finalValue; val++){
            text.text = val.ToString();
            yield return new WaitForSeconds(timeBetweenValueChanges);
        }
        nextLevelButton.SetActive(true);
    }

    [ContextMenu("LoadNextLevel")]
    public void LoadNextLevel(){
        vcam.LookAt = null;
        vcam.Follow = null;
        UICanvas.SetActive(true);
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
