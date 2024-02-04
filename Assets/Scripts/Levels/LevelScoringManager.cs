using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelScoringManager : MonoBehaviour
{
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
    public static int numCoinsCollected;
    public static int numCoinsInLevel;

    public float score;
    public SleepTimer sleepTimer;

    public void Start()
    {
        numCoinsInLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
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
    }

    [ContextMenu("CalculateScore")]
    public float CalculateScore()
    {
        float time_score = (1 - sleepTimer.timeSpent / sleepTimer.maxTime) * maxPointsForTime * timeWeight;
        if (time_score < 0){
            time_score = 0;
        }
        float coin_score = (numCoinsCollected / numCoinsInLevel) * maxPointsForCoins * coinWeight;
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

    public static void ResetNumCoinsCollected(){
        numCoinsCollected = 0;
    }
}
