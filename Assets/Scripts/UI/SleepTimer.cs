using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepTimer : MonoBehaviour
{
    public delegate void OnSleepTimerUpdate(float timeInTimer, float maxTime);
    public static event OnSleepTimerUpdate onSleepTimerUpdate;

    public delegate void OnChangeSleepValue(float changeAmt);
    public static event OnChangeSleepValue onChangeSleepValue;
    public static void CallChangeSleepValue(float amt) { onChangeSleepValue?.Invoke(amt); }

    public Slider sleepTimer;

    public float maxTime;
    public float timeInTimer;
    public float timeSpent;

    //void Awake()
    //{
    //    sleepTimer = gameObject.GetComponent<Slider>();
        
    //}

    void Start()
    {
        timeInTimer = maxTime;
        timeSpent = 0;
        onSleepTimerUpdate += UpdateSlider;
        onChangeSleepValue += ChangeSleepValue;
    }
    void OnDestroy()
    {
        onSleepTimerUpdate -= UpdateSlider;
        onChangeSleepValue -= ChangeSleepValue;
    }

    // Update is called once per frame
    void Update()
    {
        timeSpent += Time.deltaTime;
        timeInTimer -= Time.deltaTime;
        onSleepTimerUpdate?.Invoke(timeInTimer, maxTime);
    }

    private void ChangeSleepValue(float amt)
    {
        timeInTimer += amt;
        if (timeInTimer > maxTime)
        {
            timeInTimer = maxTime;
        }
        if (timeInTimer < 0)
        {
            timeInTimer = 0;
        }

        onSleepTimerUpdate.Invoke(timeInTimer, maxTime);
    }

    public void UpdateSlider(float _timeInTimer, float _maxTime)
    {
        sleepTimer.value = _timeInTimer / _maxTime;
    }
}
