using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SleepTimer : MonoBehaviour
{
    public delegate void OnSleepTimerUpdate(float currTime, float maxTime);
    public static event OnSleepTimerUpdate onSleepTimerUpdate;

    public delegate void OnChangeSleepValue(float changeAmt);
    public static event OnChangeSleepValue onChangeSleepValue;
    public static void CallChangeSleepValue(float amt) { onChangeSleepValue?.Invoke(amt); }

    public Slider sleepTimer;

    public float maxTime;
    public float currTime;

    //void Awake()
    //{
    //    sleepTimer = gameObject.GetComponent<Slider>();
        
    //}

    void Start()
    {
        currTime = maxTime;
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
        currTime -= Time.deltaTime;
        onSleepTimerUpdate?.Invoke(currTime, maxTime);
    }

    private void ChangeSleepValue(float amt)
    {
        currTime += amt;
        if (currTime > maxTime)
        {
            currTime = maxTime;
        }
        if (currTime < 0)
        {
            currTime = 0;
        }

        onSleepTimerUpdate.Invoke(currTime, maxTime);
    }

    public void UpdateSlider(float _currTime, float _maxTime)
    {
        sleepTimer.value = _currTime / _maxTime;
    }
}
