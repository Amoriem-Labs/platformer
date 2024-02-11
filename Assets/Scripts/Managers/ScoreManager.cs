using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScoreManager : MonoBehaviour
{
    public static ScoreManager Instance { get; private set; }

    public int multiplierUpgradeThreshold;
    public int maxMultiplier;
    public Image multiplierRing;
    public TMP_Text multiplierDisplayText;
    public TMP_Text scoreDisplayText;

    int currentScore;
    int multiplier = 1;
    int multiplierUpgradeScore;

    void Start()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);

        UpdateScoreboard();
    }

    public void AddPoints(int points)
    {
        currentScore += points * multiplier;
        multiplierUpgradeScore += points;

        if (multiplierUpgradeScore >= multiplierUpgradeThreshold)
        {
            if (multiplier < maxMultiplier)
            {
                multiplier *= 2;
                multiplierUpgradeScore = 0;
            }
            else
            {
                multiplierUpgradeScore = multiplierUpgradeThreshold;
            }
        }

        UpdateScoreboard();
    }

    void UpdateScoreboard()
    {
        float multiplierRingFill = multiplier == maxMultiplier ?
            1 :
            multiplierUpgradeScore / (float)multiplierUpgradeThreshold;
        multiplierRing.fillAmount = multiplierRingFill;
        scoreDisplayText.text = currentScore.ToString();
        multiplierDisplayText.text = multiplier + "x";
    }

    public void ResetMultiplier()
    {
        multiplier = 1;
        multiplierUpgradeScore = 0;
        UpdateScoreboard();
    }
}
