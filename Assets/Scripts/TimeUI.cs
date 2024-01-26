using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeUI : MonoBehaviour
{
    public TMPro.TextMeshProUGUI TimeText; 

    // called once at beginning if object has checkmark
    private void OnEnable()
    {
        TimeManager.OnMinuteChanged += UpdateTime; 
        TimeManager.OnHourChanged += UpdateTime;       
    }

    // if I ever want to pause the timer
    private void OnDisable()
    {
        TimeManager.OnMinuteChanged -= UpdateTime; 
        TimeManager.OnHourChanged -= UpdateTime; 
    }

    private void Update()
    {
    }

    private void UpdateTime()   
    {
        TimeText.text = $"{TimeManager.Hour:00}:{TimeManager.Minute:00}"; 
    }
}
